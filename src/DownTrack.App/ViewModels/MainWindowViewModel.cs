using System.Collections.ObjectModel;
using DownTrack.Application.Commands;
using DownTrack.Application.Services;
using DownTrack.Core.Models;
using DownTrack.Infrastructure.Localization;
using DownTrack.Infrastructure.Settings;

namespace DownTrack.ViewModels;

public sealed class MainWindowViewModel : ObservableObject
{
    private readonly StagingService _staging;
    private readonly ICommitService _commitService;
    private readonly ITextPromptService _textPrompt;
    private readonly IMediaDialogService _mediaDialog;
    private readonly AppSettingsService _settings;
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
        _textPrompt = textPrompt;
        _mediaDialog = mediaDialog;
        _settings = AppSettingsService.Instance;

        Home = new HomeViewModel(staging, folderPicker, toolManager);
        Home.OpenRootRequested += OpenRoot;

        _currentViewModel = Home;

        GoHomeCommand = new RelayCommand(_ => GoHome());
        AddRootFolderCommand = new RelayCommand(_ => _ = Home.AddRootFolderAsync());
        AddMediaCommand = new AsyncRelayCommand(
            AddMediaAsync,
            () => CurrentExplorer is not null);
        BackCommand = new RelayCommand(_ => CurrentExplorer?.GoBack(), _ => CurrentExplorer is not null && CurrentExplorer.CanGoBack);
        ForwardCommand = new RelayCommand(_ => CurrentExplorer?.GoForward(), _ => CurrentExplorer is not null && CurrentExplorer.CanGoForward);

        LocalizationService.Instance.PropertyChanged += (_, _) =>
        {
            OnPropertyChanged(nameof(LocationLabel));
            OnPropertyChanged(nameof(LocationPath));
            OnPropertyChanged(nameof(EngineLabel));
            BackCommand.RaiseCanExecuteChanged();
            ForwardCommand.RaiseCanExecuteChanged();
        };
    }

    public HomeViewModel Home { get; }
    public ITextPromptService TextPrompt => _textPrompt;
    public IMediaDialogService MediaDialog => _mediaDialog;

    public ObservableCollection<RootFolder> RootFolders => Home.Roots;

    public object CurrentViewModel
    {
        get => _currentViewModel;
        private set
        {
            if (!SetProperty(ref _currentViewModel, value))
                return;

            OnPropertyChanged(nameof(CurrentExplorer));
            OnPropertyChanged(nameof(IsExplorer));
            OnPropertyChanged(nameof(LocationLabel));
            OnPropertyChanged(nameof(LocationPath));
            AddMediaCommand.RaiseCanExecuteChanged();
            BackCommand.RaiseCanExecuteChanged();
            ForwardCommand.RaiseCanExecuteChanged();
        }
    }

    public ExplorerViewModel? CurrentExplorer =>
        CurrentViewModel as ExplorerViewModel;

    public bool IsExplorer => CurrentExplorer is not null;

    public string LocationLabel =>
        CurrentExplorer is null
            ? LocalizationService.Instance.T("Shell.MyLibrary")
            : CurrentExplorer.RootName;

    public string LocationPath =>
        CurrentExplorer?.CurrentPath
        ?? LocalizationService.Instance.T("Shell.AllRootFolders");

    public string EngineLabel =>
        Home.ToolReady
            ? LocalizationService.Instance.T("Shell.EngineReady")
            : LocalizationService.Instance.T("Shell.EngineNeedsSetup");

    public RelayCommand GoHomeCommand { get; }
    public RelayCommand AddRootFolderCommand { get; }
    public AsyncRelayCommand AddMediaCommand { get; }
    public RelayCommand BackCommand { get; }
    public RelayCommand ForwardCommand { get; }

    public async Task InitializeAsync()
    {
        await _staging.InitializeAsync();
        Home.Refresh();
        OnPropertyChanged(nameof(RootFolders));
    }

    public void SelectRoot(RootFolder root)
    {
        OpenRoot(root);
    }

    private async Task AddMediaAsync()
    {
        if (CurrentExplorer is null)
            return;

        await CurrentExplorer.AddMediaFromShellAsync();
        OnPropertyChanged(nameof(LocationPath));
    }

    private void GoHome()
    {
        CurrentViewModel = Home;
    }

    private void OpenRoot(RootFolder root)
    {
        var explorer = new ExplorerViewModel(
            root,
            _staging,
            _commitService,
            _textPrompt,
            _mediaDialog,
            GoHome);

        explorer.PropertyChanged += (_, e) =>
        {
            if (e.PropertyName is nameof(ExplorerViewModel.CurrentPath) or nameof(ExplorerViewModel.CanGoBack) or nameof(ExplorerViewModel.CanGoForward))
            {
                OnPropertyChanged(nameof(LocationPath));
                BackCommand.RaiseCanExecuteChanged();
                ForwardCommand.RaiseCanExecuteChanged();
            }
        };

        CurrentViewModel = explorer;
    }
}
