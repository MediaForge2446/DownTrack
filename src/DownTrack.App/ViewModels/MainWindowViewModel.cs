using DownTrack.Application.Commands;
using DownTrack.Application.Services;
using DownTrack.Core.Models;
using DownTrack.Infrastructure.Localization;

namespace DownTrack.ViewModels;

public sealed class MainWindowViewModel : ObservableObject
{
    private readonly StagingService _staging;
    private readonly ICommitService _commitService;
    private object _currentViewModel;

    public MainWindowViewModel(
        StagingService staging,
        ICommitService commitService,
        IFolderPicker folderPicker,
        ITextPromptService textPrompt,
        IMediaDialogService mediaDialog,
        IAppToolManager toolManager)
    {
        _staging = staging;
        _commitService = commitService;

        Home = new HomeViewModel(staging, folderPicker, toolManager);
        Home.OpenRootRequested += OpenRoot;

        TextPrompt = textPrompt;
        MediaDialog = mediaDialog;
        _currentViewModel = Home;

        LocalizationService.Instance.PropertyChanged += (_, e) =>
        {
            if (e.PropertyName is "Item[]" or nameof(LocalizationService.ActiveCode))
            {
                if (CurrentViewModel == Home)
                    CurrentFolderLabel = LocalizationService.Instance.T("App.Library");
                OnPropertyChanged(nameof(CurrentFolderLabel));
            }
        };

        GoHomeCommand = new RelayCommand(_ => GoHome());
    }

    public HomeViewModel Home { get; }
    public ITextPromptService TextPrompt { get; }
    public IMediaDialogService MediaDialog { get; }

    public object CurrentViewModel
    {
        get => _currentViewModel;
        private set => SetProperty(ref _currentViewModel, value);
    }

    public string CurrentFolderLabel { get; private set; } =
        LocalizationService.Instance.T("App.Library");
    public RelayCommand GoHomeCommand { get; }

    public async Task InitializeAsync()
    {
        await _staging.InitializeAsync();
        Home.Refresh();
    }

    private void GoHome()
    {
        CurrentFolderLabel = LocalizationService.Instance.T("App.Library");
        OnPropertyChanged(nameof(CurrentFolderLabel));
        Home.Refresh();
        CurrentViewModel = Home;
    }

    private void OpenRoot(RootFolder root)
    {
        CurrentFolderLabel = root.Name;
        OnPropertyChanged(nameof(CurrentFolderLabel));

        CurrentViewModel = new ExplorerViewModel(
            root,
            _staging,
            _commitService,
            TextPrompt,
            MediaDialog,
            GoHome);
    }
}
