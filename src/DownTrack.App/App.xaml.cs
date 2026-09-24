using System.Windows;
using System.Windows.Threading;
using DownTrack.Application.Services;
using DownTrack.Infrastructure;
using DownTrack.Infrastructure.Downloads;
using DownTrack.Infrastructure.Storage;
using DownTrack.Infrastructure.Tools;
using DownTrack.Infrastructure.Windows;
using DownTrack.Infrastructure.Localization;
using DownTrack.Views;

namespace DownTrack;

public partial class App : System.Windows.Application
{
    protected override async void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        ShutdownMode = ShutdownMode.OnMainWindowClose;
        DispatcherUnhandledException += OnDispatcherUnhandledException;
        LocalizationService.Instance.Initialize();

        try
        {
            await DownTrack.Infrastructure.Settings.SettingsService.Instance.InitializeAsync();
            LocalizationService.Instance.SetLanguage(
                DownTrack.Infrastructure.Settings.SettingsService.Instance.Current.LanguageCode);
            DownTrack.Infrastructure.Settings.ThemeService.Instance.Apply(
                DownTrack.Infrastructure.Settings.SettingsService.Instance.Current.Theme);

            try
        {
            var stateStore = new JsonAppStateStore(AppPaths.StateFile);
            var processRunner = new ProcessRunner();
            var locator = new ToolLocator();
            var toolManager = new ToolManager();
            var downloader = new YtDlpDownloader(locator, processRunner);
            var commitService = new CommitService(downloader, toolManager);
            var staging = new StagingService(stateStore, commitService);
            var folderPicker = new WindowsFolderPicker();
            var prompt = new WpfTextPromptService();
            var resolver = new YouTubeMetadataResolver(locator, processRunner, toolManager);
            var mediaDialog = new WpfMediaDialogService(resolver);

            var viewModel = new ViewModels.MainWindowViewModel(
                staging,
                commitService,
                folderPicker,
                prompt,
                mediaDialog,
                toolManager);

            await viewModel.InitializeAsync();

            var window = new MainWindow
            {
                DataContext = viewModel
            };

            MainWindow = window;
            window.Show();
            }
        }
        catch (Exception ex)
        {
            ShowFatalError(ex);
            Shutdown(-1);
        }
    }

    private void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
    {
        e.Handled = true;
        ShowFatalError(e.Exception);
    }

    private static void ShowFatalError(Exception ex)
    {
        MessageBox.Show(
            $"DownTrack hit an unexpected error and kept the application open when possible.\n\n{ex.Message}",
            "DownTrack",
            MessageBoxButton.OK,
            MessageBoxImage.Error);
    }
}
