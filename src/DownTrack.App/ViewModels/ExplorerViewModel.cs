using System.Collections.ObjectModel;
using DownTrack.Application.Commands;
using DownTrack.Application.Services;
using DownTrack.Core.Models;

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
    private string _statusMessage = "All changes are saved.";
    private bool _isSaving;

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
    }

    public string RootName => _root.Name;

    public string CurrentPath
    {
        get => _currentPath;
        private set => SetProperty(ref _currentPath, value);
    }

    public ObservableCollection<VirtualEntry> Entries { get; } = [];
    public ObservableCollection<VirtualEntry> Folders { get; } = [];
    public ObservableCollection<PendingChange> PendingChanges { get; } = [];

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
        ? "Save Changes  →"
        : $"Save Changes ({PendingChanges.Count})  →";

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
        foreach (var change in _staging.GetPendingChanges(_root.Id))
            PendingChanges.Add(change);

        OnPropertyChanged(nameof(SaveButtonText));

        BackCommand.RaiseCanExecuteChanged();
        ForwardCommand.RaiseCanExecuteChanged();
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
    }

    private void NavigateBack()
    {
        if (_backHistory.Count == 0)
            return;

        _forwardHistory.Push(CurrentPath);
        CurrentPath = _backHistory.Pop();
        SelectedEntry = null;
        Refresh();
    }

    private void NavigateForward()
    {
        if (_forwardHistory.Count == 0)
            return;

        _backHistory.Push(CurrentPath);
        CurrentPath = _forwardHistory.Pop();
        SelectedEntry = null;
        Refresh();
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
    }

    private async Task CreateFolderAsync()
    {
        var name = _prompt.Prompt("New folder", "Choose a name for the new folder.", "New Folder");
        if (string.IsNullOrWhiteSpace(name))
            return;

        try
        {
            await _staging.StageCreateFolderAsync(_root, CurrentPath, name);
            StatusMessage = $"Pending: create “{name.Trim()}”.";
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

        var name = _prompt.Prompt("Rename", "Enter the new name.", SelectedEntry.Name);
        if (string.IsNullOrWhiteSpace(name))
            return;

        try
        {
            await _staging.StageRenameAsync(_root, SelectedEntry.FullPath, name, SelectedEntry.IsDirectory);
            StatusMessage = $"Pending: rename “{SelectedEntry.Name}”.";
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
            ? $"Delete folder “{SelectedEntry.Name}” from the staged plan?"
            : $"Delete “{SelectedEntry.Name}” from the staged plan?";

        if (!_prompt.Confirm("Confirm delete", message))
            return;

        try
        {
            await _staging.StageDeleteAsync(_root, SelectedEntry.FullPath, SelectedEntry.IsDirectory);
            StatusMessage = $"Pending: delete “{SelectedEntry.Name}”.";
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
            ? "Media added to the pending queue."
            : $"{specs.Count} media items added to the pending queue.";

        Refresh();
    }

    private async Task SaveChangesAsync()
    {
        if (_isSaving)
            return;

        _isSaving = true;
        StatusMessage = "Applying staged changes…";
        SaveChangesCommand.RaiseCanExecuteChanged();

        try
        {
            var progress = new Progress<string>(message => StatusMessage = message);
            await _staging.SaveChangesAsync(_root, progress);
            StatusMessage = PendingChanges.Count == 0
                ? "All changes are saved."
                : "Some changes need attention.";
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
        StatusMessage = $"Cancelled: {change.Description}";
        Refresh();
    }

    public void GoHomeNow() => _goHome();
}
