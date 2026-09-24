using System.Collections.ObjectModel;
using DownTrack.Application.Commands;
using DownTrack.Core.Enums;
using DownTrack.Infrastructure.Localization;
using DownTrack.Infrastructure.Settings;

namespace DownTrack.ViewModels;

public sealed class SettingsViewModel : ObservableObject
{
    public sealed record ThemeOption(AppThemeMode Mode, string DisplayName);

    private readonly SettingsService _settings = SettingsService.Instance;
    private AppThemeMode _theme;
    private string _languageCode;
    private bool _autoUpdateApp;
    private bool _autoUpdateTools;
    private MediaFormat _defaultFormat;
    private AudioQuality _defaultAudioQuality;
    private VideoQuality _defaultVideoQuality;
    private string _updateStatus = string.Empty;

    public SettingsViewModel()
    {
        var s = _settings.Current;

        _theme = s.Theme;
        _languageCode = s.LanguageCode;
        _autoUpdateApp = s.AutoUpdateApp;
        _autoUpdateTools = s.AutoUpdateTools;
        _defaultFormat = s.DefaultFormat;
        _defaultAudioQuality = s.DefaultAudioQuality;
        _defaultVideoQuality = s.DefaultVideoQuality;

        Languages = new ObservableCollection<SupportedLanguage>(
            LocalizationService.SupportedLanguages);

        Formats = new ObservableCollection<MediaFormat>(Enum.GetValues<MediaFormat>());
        AudioQualities = new ObservableCollection<AudioQuality>(Enum.GetValues<AudioQuality>());
        VideoQualities = new ObservableCollection<VideoQuality>(Enum.GetValues<VideoQuality>());

        ThemeOptions = new ObservableCollection<ThemeOption>();
        RebuildThemeOptions();

        ApplyCommand = new AsyncRelayCommand(SaveAsync);
        CheckForUpdatesCommand = new AsyncRelayCommand(CheckForUpdatesAsync);
        CheckForToolUpdatesCommand = new AsyncRelayCommand(CheckForToolUpdatesAsync);

        LocalizationService.Instance.PropertyChanged += Localization_PropertyChanged;
    }

    public ObservableCollection<SupportedLanguage> Languages { get; }
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
            RebuildThemeOptions();
            OnPropertyChanged(nameof(LanguageCode));
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
    public AsyncRelayCommand CheckForToolUpdatesCommand { get; }

    private async Task SaveAsync()
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

    private Task CheckForUpdatesAsync()
    {
        UpdateStatus = LocalizationService.Instance.T("Updates.Checking");
        return Task.Delay(250);
    }

    private Task CheckForToolUpdatesAsync()
    {
        UpdateStatus = _settings.Current.AutoUpdateTools
            ? LocalizationService.Instance.T("Settings.ToolsReady")
            : LocalizationService.Instance.T("Settings.ToolsMissing");
        return Task.Delay(250);
    }

    private void Localization_PropertyChanged(
        object? sender,
        System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (e.PropertyName is "Item[]" or nameof(LocalizationService.ActiveCode))
        {
            RebuildThemeOptions();
            OnPropertyChanged(nameof(UpdateStatus));
        }
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
