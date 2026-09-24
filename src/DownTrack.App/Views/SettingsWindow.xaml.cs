using System.Windows;
using System.Windows.Input;
using DownTrack.Core.Enums;
using DownTrack.Infrastructure.Localization;
using DownTrack.Infrastructure.Settings;
using DownTrack.Infrastructure.Updates;

namespace DownTrack.Views;

public partial class SettingsWindow : Window
{
    private sealed record ThemeOption(string Code, string Label);
    private sealed record LanguageOption(string Code, string DisplayName);

    private readonly AppSettingsService _settings;
    private readonly IAppToolManager _toolManager;
    private AppUpdateInfo? _appUpdate;
    private bool _loading;

    public SettingsWindow(IAppToolManager toolManager)
    {
        InitializeComponent();

        _settings = AppSettingsService.Instance;
        _toolManager = toolManager;

        LocalizationService.Instance.PropertyChanged += Localization_PropertyChanged;
        Loaded += SettingsWindow_Loaded;
        Closed += SettingsWindow_Closed;
    }

    private void SettingsWindow_Loaded(object sender, RoutedEventArgs e)
    {
        _loading = true;
        try
        {
            PopulateThemeOptions();
            PopulateLanguageOptions();

            FormatBox.ItemsSource = Enum.GetValues<MediaFormat>();
            AudioQualityBox.ItemsSource = Enum.GetValues<AudioQuality>();
            VideoQualityBox.ItemsSource = Enum.GetValues<VideoQuality>();

            ThemeBox.SelectedValue = _settings.Current.Theme;
            LanguageBox.SelectedValue = LocalizationService.Instance.SelectedCode;
            FormatBox.SelectedItem = Enum.TryParse<MediaFormat>(_settings.Current.DefaultFormat, true, out var format)
                ? format
                : MediaFormat.Mp3;
            AudioQualityBox.SelectedItem = Enum.IsDefined(typeof(AudioQuality), _settings.Current.DefaultAudioQuality)
                ? (AudioQuality)_settings.Current.DefaultAudioQuality
                : AudioQuality.Kbps128;
            VideoQualityBox.SelectedItem = Enum.IsDefined(typeof(VideoQuality), _settings.Current.DefaultVideoQuality)
                ? (VideoQuality)_settings.Current.DefaultVideoQuality
                : VideoQuality.P720;

            AutoUpdateBox.IsChecked = _settings.Current.AutoUpdate;
            AutoToolUpdateBox.IsChecked = _settings.Current.AutoUpdateTools;

            AppUpdateStatus.Text = AppUpdateService.Instance.CurrentVersion;
            ToolUpdateStatus.Text = LocalizationService.Instance.T("Settings.ToolsReady");
        }
        finally
        {
            _loading = false;
        }
    }

    private void PopulateThemeOptions()
    {
        ThemeBox.ItemsSource =
        [
            new ThemeOption("System", LocalizationService.Instance.T("Settings.ThemeSystem")),
            new ThemeOption("Light", LocalizationService.Instance.T("Settings.ThemeLight")),
            new ThemeOption("Dark", LocalizationService.Instance.T("Settings.ThemeDark"))
        ];
    }

    private void PopulateLanguageOptions()
    {
        LanguageBox.ItemsSource = LocalizationService.SupportedLanguages
            .Select(x => new LanguageOption(
                x.Code,
                x.Code == "auto"
                    ? LocalizationService.Instance.T("Settings.Automatic")
                    : x.NativeName))
            .ToList();
    }

    private void Localization_PropertyChanged(
        object? sender,
        System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (e.PropertyName is "Item[]" or nameof(LocalizationService.ActiveCode))
        {
            _loading = true;
            try
            {
                PopulateThemeOptions();
                PopulateLanguageOptions();
                ThemeBox.SelectedValue = _settings.Current.Theme;
                LanguageBox.SelectedValue = LocalizationService.Instance.SelectedCode;
            }
            finally
            {
                _loading = false;
            }
        }
    }

    private void ThemeBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
    {
        if (_loading || ThemeBox.SelectedValue is not string theme)
            return;

        ThemeService.Instance.Apply(theme);
        _settings.Update(x => x.Theme = theme);
    }

    private void LanguageBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
    {
        if (_loading || LanguageBox.SelectedValue is not string code)
            return;

        LocalizationService.Instance.SetLanguage(code);
        _settings.Update(x => x.Language = code);
    }

    private async void CheckApp_Click(object sender, RoutedEventArgs e)
    {
        CheckAppButton.IsEnabled = false;
        try
        {
            AppUpdateStatus.Text = LocalizationService.Instance.T("Updates.Checking");
            _appUpdate = await AppUpdateService.Instance.CheckAsync();

            AppUpdateStatus.Text = _appUpdate.Message;
            UpdateAppButton.IsEnabled = _appUpdate.IsUpdateAvailable;
        }
        catch (Exception ex)
        {
            AppUpdateStatus.Text = LocalizationService.Instance.T(
                "Updates.CheckFailed",
                ex.Message);
            UpdateAppButton.IsEnabled = false;
        }
        finally
        {
            CheckAppButton.IsEnabled = true;
        }
    }

    private async void UpdateApp_Click(object sender, RoutedEventArgs e)
    {
        if (_appUpdate is not { IsUpdateAvailable: true })
            return;

        UpdateAppButton.IsEnabled = false;
        try
        {
            AppUpdateStatus.Text = LocalizationService.Instance.T("Updates.Downloading");
            await AppUpdateService.Instance.InstallAsync(_appUpdate);
            AppUpdateStatus.Text = LocalizationService.Instance.T("Updates.Restarting");
        }
        catch (Exception ex)
        {
            AppUpdateStatus.Text = LocalizationService.Instance.T(
                "Updates.CheckFailed",
                ex.Message);
            UpdateAppButton.IsEnabled = true;
        }
    }

    private async void CheckTools_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            ToolUpdateStatus.Text = _toolManager.IsReady
                ? LocalizationService.Instance.T("Settings.ToolsReady")
                : LocalizationService.Instance.T("Settings.ToolsMissing");
        }
        }
    }

    private async void UpdateTools_Click(object sender, RoutedEventArgs e)
    {
        UpdateToolsButton.IsEnabled = false;
        try
        {
            var progress = new Progress<string>(message => ToolUpdateStatus.Text = message);
            await _toolManager.UpdateAsync(progress);
            ToolUpdateStatus.Text = LocalizationService.Instance.T("Settings.ToolsUpdated");
        }
        catch (Exception ex)
        {
            ToolUpdateStatus.Text = LocalizationService.Instance.T(
                "Updates.CheckFailed",
                ex.Message);
        }
        finally
        {
            UpdateToolsButton.IsEnabled = true;
        }
    }

    private void Save_Click(object sender, RoutedEventArgs e)
    {
        if (FormatBox.SelectedItem is MediaFormat format &&
            AudioQualityBox.SelectedItem is AudioQuality audio &&
            VideoQualityBox.SelectedItem is VideoQuality video)
        {
            _settings.Update(s =>
            {
                s.Theme = ThemeBox.SelectedValue as string ?? "System";
                s.Language = LanguageBox.SelectedValue as string ?? "auto";
                s.AutoUpdate = AutoUpdateBox.IsChecked == true;
                s.AutoUpdateTools = AutoToolUpdateBox.IsChecked == true;
                s.DefaultFormat = format.ToString();
                s.DefaultAudioQuality = (int)audio;
                s.DefaultVideoQuality = (int)video;
            });

            ThemeService.Instance.Apply(_settings.Current.Theme);
            LocalizationService.Instance.SetLanguage(_settings.Current.Language);
        }

        DialogResult = true;
    }

    private void Close_Click(object sender, RoutedEventArgs e) =>
        DialogResult = false;

    private void SettingsWindow_Closed(object? sender, EventArgs e) =>
        LocalizationService.Instance.PropertyChanged -= Localization_PropertyChanged;

    private void TitleBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        if (e.OriginalSource is System.Windows.Controls.Button)
            return;

        if (e.LeftButton == MouseButtonState.Pressed)
            DragMove();
    }
}