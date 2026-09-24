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

        GoHomeCommand = new RelayCommand(_ => GoHome());
        LocalizationManager.Instance.LanguageChanged += (_, _) =>
        {
            if (CurrentViewModel == Home)
                CurrentFolderLabel = LocalizationManager.Instance["App.Library"];

            OnPropertyChanged(nameof(CurrentFolderLabel));
        };
    }

    public HomeViewModel Home { get; }
    public ITextPromptService TextPrompt { get; }
    public IMediaDialogService MediaDialog { get; }

    public object CurrentViewModel
    {
        get => _currentViewModel;
        private set => SetProperty(ref _currentViewModel, value);
    }

    public string CurrentFolderLabel { get; private set; } = LocalizationManager.Instance["App.Library"];
    public RelayCommand GoHomeCommand { get; }

    public async Task InitializeAsync()
    {
        await _staging.InitializeAsync();
        Home.Refresh();
    }

    private void GoHome()
    {
        CurrentFolderLabel = LocalizationManager.Instance["App.Library"];
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
