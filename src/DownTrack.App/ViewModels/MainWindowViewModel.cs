using DownTrack.Application.Commands;
using DownTrack.Application.Services;
using DownTrack.Core.Models;
using DownTrack.Infrastructure.Localization;

namespace DownTrack.ViewModels;

public sealed class MainWindowViewModel : ObservableObject
{
    private readonly StagingService _staging;
    private readonly ICommitService _commitService;
    private readonly ITextPromptService _textPrompt;
    private readonly IMediaDialogService _mediaDialog;
    private ExplorerViewModel? _currentExplorer;
    private object _currentViewModel;
    private RootFolder? _currentRoot;

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
        ToolManager = toolManager;

        Home = new HomeViewModel(staging, folderPicker, toolManager, textPrompt);
        Home.OpenRootRequested += OpenRoot;

        _currentViewModel = Home;

        LocalizationService.Instance.PropertyChanged += (_, e) =>
        {
            if (e.PropertyName is "Item[]" or nameof(LocalizationService.ActiveCode))
            {
                OnPropertyChanged(nameof(BreadcrumbText));
                OnPropertyChanged(nameof(CurrentFolderLabel));
            }
        };

        OpenRootCommand = new RelayCommand(
            parameter =>
            {
                if (parameter is RootFolder root)
                    OpenRoot(root);
            });

        AddRootFolderCommand = new RelayCommand(_ => _ = Home.AddRootFolderAsync());

        NewFolderCommand = new RelayCommand(
            _ =>
            {
                if (_currentExplorer is not null)
                    _currentExplorer.CreateFolder();
                else
                    _ = Home.AddRootFolderAsync();
            });

        AddMediaCommand = new AsyncRelayCommand(
            AddMediaAsync,
            () => _currentExplorer is not null || Home.Roots.Count > 0);

        HomeCommand = new RelayCommand(_ => GoHome());
        BackCommand = new RelayCommand(
            _ => _currentExplorer?.NavigateBackFromShell(),
            _ => _currentExplorer?.CanGoBack == true);

        ForwardCommand = new RelayCommand(
            _ => _currentExplorer?.NavigateForwardFromShell(),
            _ => _currentExplorer?.CanGoForward == true);
    }

    public HomeViewModel Home { get; }
    public IAppToolManager ToolManager { get; }

    public object CurrentViewModel
    {
        get => _currentViewModel;
        private set => SetProperty(ref _currentViewModel, value);
    }

    public RootFolder? CurrentRoot
    {
        get => _currentRoot;
        private set => SetProperty(ref _currentRoot, value);
    }

    public bool IsInLibraryHome => _currentExplorer is null;

    public string CurrentFolderLabel =>
        CurrentRoot?.Name ?? LocalizationService.Instance.T("Shell.Library");

    public string BreadcrumbText
    {
        get
        {
            if (_currentExplorer is null)
                return $"{LocalizationService.Instance.T("Shell.MyLibrary")} / {LocalizationService.Instance.T("Shell.AllRootFolders")}";

            return _currentExplorer.CurrentPath;
        }
    }

    public RelayCommand OpenRootCommand { get; }
    public RelayCommand AddRootFolderCommand { get; }
    public RelayCommand NewFolderCommand { get; }
    public AsyncRelayCommand AddMediaCommand { get; }
    public RelayCommand HomeCommand { get; }
    public RelayCommand BackCommand { get; }
    public RelayCommand ForwardCommand { get; }

    public async Task InitializeAsync()
    {
        await _staging.InitializeAsync();
        Home.Refresh();
    }

    private void GoHome()
    {
        _currentExplorer = null;
        CurrentRoot = null;
        Home.SelectedRoot = null;
        Home.Refresh();

        CurrentViewModel = Home;

        OnPropertyChanged(nameof(IsInLibraryHome));
        OnPropertyChanged(nameof(BreadcrumbText));
        OnPropertyChanged(nameof(CurrentFolderLabel));
        BackCommand.RaiseCanExecuteChanged();
        ForwardCommand.RaiseCanExecuteChanged();
        AddMediaCommand.RaiseCanExecuteChanged();
        NewFolderCommand.RaiseCanExecuteChanged();
    }

    private void OpenRoot(RootFolder root)
    {
        _currentRoot = root;
        CurrentRoot = root;
        Home.SelectedRoot = root;

        var explorer = new ExplorerViewModel(
            root,
            _staging,
            _commitService,
            _textPrompt,
            _mediaDialog,
            GoHome);

        explorer.PropertyChanged += Explorer_PropertyChanged;
        explorer.NavigationChanged += Explorer_NavigationChanged;

        _currentExplorer = explorer;
        CurrentViewModel = explorer;

        OnPropertyChanged(nameof(IsInLibraryHome));
        OnPropertyChanged(nameof(BreadcrumbText));
        OnPropertyChanged(nameof(CurrentFolderLabel));

        BackCommand.RaiseCanExecuteChanged();
        ForwardCommand.RaiseCanExecuteChanged();
        AddMediaCommand.RaiseCanExecuteChanged();
        NewFolderCommand.RaiseCanExecuteChanged();
    }

    private async Task AddMediaAsync()
    {
        if (_currentExplorer is null)
        {
            var root = Home.Roots.FirstOrDefault();
            if (root is null)
                return;

            OpenRoot(root);
        }

        _currentExplorer?.AddMedia();
        await Task.CompletedTask;
    }

    private void Explorer_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (e.PropertyName is nameof(ExplorerViewModel.CanGoBack) or nameof(ExplorerViewModel.CanGoForward))
        {
            BackCommand.RaiseCanExecuteChanged();
            ForwardCommand.RaiseCanExecuteChanged();
        }

        if (e.PropertyName == nameof(ExplorerViewModel.CurrentPath))
        {
            OnPropertyChanged(nameof(BreadcrumbText));
            OnPropertyChanged(nameof(CurrentFolderLabel));
        }
    }

    private void Explorer_NavigationChanged()
    {
        OnPropertyChanged(nameof(BreadcrumbText));
        BackCommand.RaiseCanExecuteChanged();
        ForwardCommand.RaiseCanExecuteChanged();
    }
}
