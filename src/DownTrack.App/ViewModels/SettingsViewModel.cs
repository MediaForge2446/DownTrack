using System.Collections.ObjectModel;
using DownTrack.Application.Commands;
using DownTrack.Application.Services;
using DownTrack.Core.Enums;
using DownTrack.Infrastructure.Localization;
using DownTrack.Infrastructure.Settings;
using DownTrack.Infrastructure.Updates;

namespace DownTrack.ViewModels;

public sealed class SettingsViewModel : ObservableObject
{
    public sealed record ThemeOption(AppThemeMode Mode, string DisplayName);
    public sealed record LanguageOption(string Code, string DisplayName);

    private readonly SettingsService _settings = SettingsService.Instance;
    private readonly IAppToolManager _toolManager;
    private AppThemeMode _theme;
    private string _languageCode;
    private bool _autoUpdateApp;
    private bool _autoUpdateTools;
    private MediaFormat _defaultFormat;
    private AudioQuality _defaultAudioQuality;
    private VideoQuality _defaultVideoQuality;
    private string _updateStatus = string.Empty;
    private AppUpdateInfo? _availableUpdate;

    public SettingsViewModel(IAppToolManager toolManager)
    {
        _toolManager = toolManager;

        var s = _settings.Current;

        _theme = s.Theme;
        _languageCode = s.LanguageCode;
        _autoUpdateApp = s.AutoUpdateApp;
        _autoUpdateTools = s.AutoUpdateTools;
        _defaultFormat = s.DefaultFormat;
        _defaultAudioQuality = s.DefaultAudioQuality;
        _defaultVideoQuality = s.DefaultVideoQuality;

        Languages = new ObservableCollection<LanguageOption>(
            LocalizationService.SupportedLanguages.Select(ToLanguageOption));

        Formats = new ObservableCollection<MediaFormat>(Enum.GetValues<MediaFormat>());
        AudioQualities = new ObservableCollection<AudioQuality>(Enum.GetValues<AudioQuality>());
        VideoQualities = new ObservableCollection<VideoQuality>(Enum.GetValues<VideoQuality>());

        ThemeOptions = new ObservableCollection<ThemeOption>();
        RebuildThemeOptions();

        ApplyCommand = new AsyncRelayCommand(SaveAsync);
        CheckForUpdatesCommand = new AsyncRelayCommand(CheckForUpdatesAsync);
        UpdateAppCommand = new AsyncRelayCommand(UpdateAppAsync, () => _availableUpdate?.IsUpdateAvailable == true);
        CheckForToolUpdatesCommand = new AsyncRelayCommand(CheckForToolUpdatesAsync);
        UpdateToolsCommand = new AsyncRelayCommand(UpdateToolsAsync);

        LocalizationService.Instance.PropertyChanged += Localization_PropertyChanged;
    }

    public ObservableCollection<LanguageOption> Languages { get; }
    public ObservableCollection<MediaFormat> Formats { get; }
    public ObservableCollection<AudioQuality> AudioQualities { get; }
    public ObservableCollection<VideoQuality> VideoQualities { get; }
    public ObservableCollection<ThemeOption> ThemeOptions { get; }

    public AppThemeMode Theme
    {
        get => _theme;
        set
        {
            if (!SetProperty(ref _theme, value))
                return;

            ThemeService.Instance.Apply(value);
        }
    }

    public string LanguageCode
    {
        get => _languageCode;
        set
        {
            if (!SetProperty(ref _languageCode, value))
                return;

            LocalizationService.Instance.SetLanguage(value);
        }
    }

    public bool AutoUpdateApp
    {
        get => _autoUpdateApp;
        set => SetProperty(ref _autoUpdateApp, value);
    }

    public bool AutoUpdateTools
    {
        get => _autoUpdateTools;
        set => SetProperty(ref _autoUpdateTools, value);
    }

    public MediaFormat DefaultFormat
    {
        get => _defaultFormat;
        set => SetProperty(ref _defaultFormat, value);
    }

    public AudioQuality DefaultAudioQuality
    {
        get => _defaultAudioQuality;
        set => SetProperty(ref _defaultAudioQuality, value);
    }

    public VideoQuality DefaultVideoQuality
    {
        get => _defaultVideoQuality;
        set => SetProperty(ref _defaultVideoQuality, value);
    }

    public string UpdateStatus
    {
        get => _updateStatus;
        private set => SetProperty(ref _updateStatus, value);
    }

    public AsyncRelayCommand ApplyCommand { get; }
    public AsyncRelayCommand CheckForUpdatesCommand { get; }
    public AsyncRelayCommand UpdateAppCommand { get; }
    public AsyncRelayCommand CheckForToolUpdatesCommand { get; }
    public AsyncRelayCommand UpdateToolsCommand { get; }

    public async Task SaveAsync()
    {
        await _settings.UpdateAsync(s =>
        {
            s.Theme = Theme;
            s.LanguageCode = LanguageCode;
            s.AutoUpdateApp = AutoUpdateApp;
            s.AutoUpdateTools = AutoUpdateTools;
            s.DefaultFormat = DefaultFormat;
            s.DefaultAudioQuality = DefaultAudioQuality;
            s.DefaultVideoQuality = DefaultVideoQuality;
        });

        LocalizationService.Instance.SetLanguage(LanguageCode);
        ThemeService.Instance.Apply(Theme);
        UpdateStatus = LocalizationService.Instance.T("Settings.Saved");
    }

    private async Task CheckForUpdatesAsync()
    {
        UpdateStatus = LocalizationService.Instance.T("Updates.Checking");

        try
        {
            _availableUpdate = await AppUpdateService.Instance.CheckAsync();
            UpdateStatus = _availableUpdate.IsUpdateAvailable
                ? LocalizationService.Instance.T("Updates.Available", _availableUpdate.Version)
                : LocalizationService.Instance.T("Updates.UpToDate");

            UpdateAppCommand.RaiseCanExecuteChanged();
        }
        catch (Exception ex)
        {
            UpdateStatus = LocalizationService.Instance.T(
                "Updates.CheckFailed",
                ex.Message);
        }
    }

    private async Task UpdateAppAsync()
    {
        if (_availableUpdate is null || !_availableUpdate.IsUpdateAvailable)
            return;

        UpdateStatus = LocalizationService.Instance.T("Updates.Downloading");

        try
        {
            await AppUpdateService.Instance.ApplyAsync(_availableUpdate);
            UpdateStatus = LocalizationService.Instance.T("Updates.Restarting");
            await Task.Delay(400);
            System.Windows.Application.Current.Shutdown();
        }
        catch (Exception ex)
        {
            UpdateStatus = LocalizationService.Instance.T(
                "Updates.CheckFailed",
                ex.Message);
        }
    }

    private async Task CheckForToolUpdatesAsync()
    {
        UpdateStatus = LocalizationService.Instance.T("Updates.Checking");

        try
        {
            UpdateStatus = _toolManager.IsReady
                ? LocalizationService.Instance.T("Settings.ToolsReady")
                : LocalizationService.Instance.T("Settings.ToolsMissing");
        }
        catch (Exception ex)
        {
            UpdateStatus = ex.Message;
        }

        await Task.CompletedTask;
    }

    private async Task UpdateToolsAsync()
    {
        UpdateStatus = LocalizationService.Instance.T("Engine.UpdatingYtDlp");

        try
        {
            var progress = new Progress<string>(message => UpdateStatus = message);
            await _toolManager.UpdateAsync(progress);
            UpdateStatus = LocalizationService.Instance.T("Settings.ToolsUpdated");
        }
        catch (Exception ex)
        {
            UpdateStatus = ex.Message;
        }
    }

    private void Localization_PropertyChanged(
        object? sender,
        System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (e.PropertyName is "Item[]" or nameof(LocalizationService.ActiveCode))
        {
            RebuildThemeOptions();

            Languages.Clear();
            foreach (var language in LocalizationService.SupportedLanguages)
                Languages.Add(ToLanguageOption(language));

            OnPropertyChanged(nameof(UpdateStatus));
        }
    }

    private LanguageOption ToLanguageOption(SupportedLanguage language)
    {
        return new LanguageOption(
            language.Code,
            language.Code == "auto"
                ? LocalizationService.Instance.T("Settings.Automatic")
                : language.NativeName);
    }

    private void RebuildThemeOptions()
    {
        ThemeOptions.Clear();

        ThemeOptions.Add(new ThemeOption(
            AppThemeMode.System,
            LocalizationService.Instance.T("Settings.ThemeSystem")));

        ThemeOptions.Add(new ThemeOption(
            AppThemeMode.Light,
            LocalizationService.Instance.T("Settings.ThemeLight")));

        ThemeOptions.Add(new ThemeOption(
            AppThemeMode.Dark,
            LocalizationService.Instance.T("Settings.ThemeDark")));
    }
}
