using System.Collections.ObjectModel;
using DownTrack.Application.Commands;
using DownTrack.Application.Services;
using DownTrack.Core.Models;
using DownTrack.Infrastructure.Localization;

namespace DownTrack.ViewModels;

public sealed class ExplorerViewModel : ObservableObject
{
    private readonly RootFolder _root;
    private readonly StagingService _staging;
    private readonly ICommitService _commitService;
    private readonly ITextPromptService _prompt;
    private readonly IMediaDialogService _mediaDialog;
    private readonly Action _goHome;
    private readonly Stack<string> _backHistory = [];
    private readonly Stack<string> _forwardHistory = [];
    private string _currentPath;
    private VirtualEntry? _selectedEntry;
    private string _statusMessage = LocalizationService.Instance.T("Explorer.AllSaved");
    private bool _isSaving;
    private double _totalProgress;

    public ExplorerViewModel(
        RootFolder root,
        StagingService staging,
        ICommitService commitService,
        ITextPromptService prompt,
        IMediaDialogService mediaDialog,
        Action goHome)
    {
        _root = root;
        _staging = staging;
        _commitService = commitService;
        _prompt = prompt;
        _mediaDialog = mediaDialog;
        _goHome = goHome;
        _currentPath = root.Path;

        BackCommand = new RelayCommand(_ => NavigateBack(), _ => _backHistory.Count > 0);
        ForwardCommand = new RelayCommand(_ => NavigateForward(), _ => _forwardHistory.Count > 0);
        HomeCommand = new RelayCommand(_ => GoToRoot());
        NewFolderCommand = new AsyncRelayCommand(CreateFolderAsync);
        RenameCommand = new AsyncRelayCommand(RenameAsync, () => SelectedEntry is not null && !_isSaving);
        DeleteCommand = new AsyncRelayCommand(DeleteAsync, () => SelectedEntry is not null && !_isSaving);
        AddMediaCommand = new AsyncRelayCommand(AddMediaAsync, () => !_isSaving);
        SaveChangesCommand = new AsyncRelayCommand(SaveChangesAsync, () => PendingChanges.Count > 0 && !_isSaving);

        Refresh();

        LocalizationService.Instance.PropertyChanged += (_, e) =>
        {
            if (e.PropertyName is "Item[]" or nameof(LocalizationService.ActiveCode))
            {
                OnPropertyChanged(nameof(SaveButtonText));
                OnPropertyChanged(nameof(PendingCountText));

                if (PendingChanges.Count == 0)
                    StatusMessage = LocalizationService.Instance.T("Explorer.AllSaved");
            }
        };
    }

    public string RootName => _root.Name;

    public bool CanGoBack => _backHistory.Count > 0;
    public bool CanGoForward => _forwardHistory.Count > 0;

    public string CurrentPath
    {
        get => _currentPath;
        private set => SetProperty(ref _currentPath, value);
    }

    public ObservableCollection<VirtualEntry> Entries { get; } = [];
    public ObservableCollection<VirtualEntry> Folders { get; } = [];
    public ObservableCollection<PendingChange> PendingChanges { get; } = [];
    public ObservableCollection<PendingChangeItemViewModel> PendingItems { get; } = [];

    public double TotalProgress
    {
        get => _totalProgress;
        private set => SetProperty(ref _totalProgress, value);
    }

    public VirtualEntry? SelectedEntry
    {
        get => _selectedEntry;
        set
        {
            if (SetProperty(ref _selectedEntry, value))
            {
                RenameCommand.RaiseCanExecuteChanged();
                DeleteCommand.RaiseCanExecuteChanged();
            }
        }
    }

    public string StatusMessage
    {
        get => _statusMessage;
        private set => SetProperty(ref _statusMessage, value);
    }

    public string SaveButtonText => PendingChanges.Count == 0
        ? LocalizationService.Instance.T("Explorer.SaveChangesArrow")
        : LocalizationService.Instance.T("Explorer.SaveChangesCount", PendingChanges.Count);

    public string PendingCountText =>
        LocalizationService.Instance.T(
            PendingChanges.Count == 1
                ? "Explorer.QueuedOne"
                : "Explorer.Queued",
            PendingChanges.Count);

    public RelayCommand BackCommand { get; }
    public RelayCommand ForwardCommand { get; }
    public RelayCommand HomeCommand { get; }
    public AsyncRelayCommand NewFolderCommand { get; }
    public AsyncRelayCommand RenameCommand { get; }
    public AsyncRelayCommand DeleteCommand { get; }
    public AsyncRelayCommand AddMediaCommand { get; }
    public AsyncRelayCommand SaveChangesCommand { get; }

    public void Refresh()
    {
        Entries.Clear();
        Folders.Clear();

        foreach (var entry in _staging.GetEntries(_root, CurrentPath))
            Entries.Add(entry);

        foreach (var folder in Entries.Where(x => x.IsDirectory))
            Folders.Add(folder);

        PendingChanges.Clear();
        PendingItems.Clear();

        foreach (var change in _staging.GetPendingChanges(_root.Id))
        {
            PendingChanges.Add(change);
            PendingItems.Add(new PendingChangeItemViewModel(change));
        }

        TotalProgress = PendingItems.Count == 0
            ? 100
            : PendingItems.Average(x => x.Progress);

        OnPropertyChanged(nameof(SaveButtonText));
        OnPropertyChanged(nameof(PendingCountText));

        BackCommand.RaiseCanExecuteChanged();
        ForwardCommand.RaiseCanExecuteChanged();
        OnPropertyChanged(nameof(CanGoBack));
        OnPropertyChanged(nameof(CanGoForward));
        RenameCommand.RaiseCanExecuteChanged();
        DeleteCommand.RaiseCanExecuteChanged();
        AddMediaCommand.RaiseCanExecuteChanged();
        SaveChangesCommand.RaiseCanExecuteChanged();
    }

    public void NavigateTo(VirtualEntry entry)
    {
        if (!entry.IsDirectory)
            return;

        _backHistory.Push(CurrentPath);
        _forwardHistory.Clear();
        CurrentPath = entry.FullPath;
        SelectedEntry = null;
        Refresh();
        NavigationChanged?.Invoke();
    }

    public void NavigateBackFromShell() => NavigateBack();

    public void NavigateForwardFromShell() => NavigateForward();

    public void CreateFolder() => _ = CreateFolderAsync();

    public void AddMedia() => _ = AddMediaAsync();

    private void NavigateBack()
    {
        if (_backHistory.Count == 0)
            return;

        _forwardHistory.Push(CurrentPath);
        CurrentPath = _backHistory.Pop();
        SelectedEntry = null;
        Refresh();
        NavigationChanged?.Invoke();
    }

    private void NavigateForward()
    {
        if (_forwardHistory.Count == 0)
            return;

        _backHistory.Push(CurrentPath);
        CurrentPath = _forwardHistory.Pop();
        SelectedEntry = null;
        Refresh();
        NavigationChanged?.Invoke();
    }

    private void GoToRoot()
    {
        if (string.Equals(CurrentPath, _root.Path, StringComparison.OrdinalIgnoreCase))
            return;

        _backHistory.Push(CurrentPath);
        _forwardHistory.Clear();
        CurrentPath = _root.Path;
        SelectedEntry = null;
        Refresh();
        NavigationChanged?.Invoke();
    }

    private async Task CreateFolderAsync()
    {
        var name = _prompt.Prompt(LocalizationService.Instance.T("Explorer.NewFolder"), LocalizationService.Instance.T("Explorer.NewFolderPrompt"), LocalizationService.Instance.T("Explorer.NewFolderDefault"));
        if (string.IsNullOrWhiteSpace(name))
            return;

        try
        {
            await _staging.StageCreateFolderAsync(_root, CurrentPath, name);
            StatusMessage = LocalizationService.Instance.T("Explorer.PendingCreate", name.Trim());
            Refresh();
        }
        catch (Exception ex)
        {
            StatusMessage = ex.Message;
        }
    }

    private async Task RenameAsync()
    {
        if (SelectedEntry is null)
            return;

        var name = _prompt.Prompt(LocalizationService.Instance.T("Explorer.Rename"), LocalizationService.Instance.T("Explorer.RenamePrompt"), SelectedEntry.Name);
        if (string.IsNullOrWhiteSpace(name))
            return;

        try
        {
            await _staging.StageRenameAsync(_root, SelectedEntry.FullPath, name, SelectedEntry.IsDirectory);
            StatusMessage = LocalizationService.Instance.T("Explorer.PendingRename", SelectedEntry.Name);
            Refresh();
        }
        catch (Exception ex)
        {
            StatusMessage = ex.Message;
        }
    }

    private async Task DeleteAsync()
    {
        if (SelectedEntry is null)
            return;

        var message = SelectedEntry.IsDirectory
            ? LocalizationService.Instance.T("Explorer.DeleteFolderPrompt", SelectedEntry.Name)
            : LocalizationService.Instance.T("Explorer.DeleteFilePrompt", SelectedEntry.Name);

        if (!_prompt.Confirm(LocalizationService.Instance.T("Explorer.ConfirmDelete"), message))
            return;

        try
        {
            await _staging.StageDeleteAsync(_root, SelectedEntry.FullPath, SelectedEntry.IsDirectory);
            StatusMessage = LocalizationService.Instance.T("Explorer.PendingDelete", SelectedEntry.Name);
            SelectedEntry = null;
            Refresh();
        }
        catch (Exception ex)
        {
            StatusMessage = ex.Message;
        }
    }

    private async Task AddMediaAsync()
    {
        var specs = _mediaDialog.Show(CurrentPath);
        if (specs.Count == 0)
            return;

        foreach (var spec in specs)
            await _staging.StageDownloadAsync(_root, CurrentPath, spec);

        StatusMessage = specs.Count == 1
            ? LocalizationService.Instance.T("Explorer.MediaAddedOne")
            : LocalizationService.Instance.T("Explorer.MediaAddedMany", specs.Count);

        Refresh();
    }

    private async Task SaveChangesAsync()
    {
        if (_isSaving)
            return;

        _isSaving = true;
        StatusMessage = LocalizationService.Instance.T("Explorer.ApplyingChanges");
        SaveChangesCommand.RaiseCanExecuteChanged();

        try
        {
            var progress = new Progress<StagedChangeProgress>(update =>
            {
                var item = PendingItems.FirstOrDefault(x => x.Change.Id == update.ChangeId);
                item?.Apply(update);

                var completed = update.CompletedChanges;
                var current = update.TotalChanges > 0
                    ? update.CurrentProgress / 100d
                    : 0d;

                TotalProgress = update.TotalChanges <= 0
                    ? 100
                    : Math.Clamp(
                        ((completed + current) / update.TotalChanges) * 100d,
                        0,
                        100);

                StatusMessage = update.StatusText;
                OnPropertyChanged(nameof(SaveButtonText));
                OnPropertyChanged(nameof(PendingCountText));
            });

            await _staging.SaveChangesAsync(_root, progress);
            TotalProgress = PendingChanges.Count == 0 ? 100 : TotalProgress;

            StatusMessage = PendingChanges.Count == 0
                ? LocalizationService.Instance.T("Explorer.AllSaved")
                : LocalizationService.Instance.T("Explorer.SomeNeedAttention");
        }
        catch (Exception ex)
        {
            StatusMessage = ex.Message;
        }
        finally
        {
            _isSaving = false;
            Refresh();
        }
    }

    public void CancelPending(PendingChange change)
    {
        _ = CancelPendingAsync(change);
    }

    private async Task CancelPendingAsync(PendingChange change)
    {
        await _staging.CancelChangeAsync(_root.Id, change.Id);
        StatusMessage = LocalizationService.Instance.T("Explorer.Cancelled", change.Description);
        Refresh();
    }

    public void GoHomeNow() => _goHome();
}
