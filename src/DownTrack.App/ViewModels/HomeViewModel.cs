using System.Collections.ObjectModel;
using DownTrack.Application.Commands;
using DownTrack.Application.Services;
using DownTrack.Core.Models;
using DownTrack.Infrastructure.Localization;

namespace DownTrack.ViewModels;

public sealed class HomeViewModel : ObservableObject
{
    private readonly StagingService _staging;
    private readonly IFolderPicker _folderPicker;
    private readonly IAppToolManager _toolManager;
    private RootFolder? _selectedRoot;
    private string _toolStatus = string.Empty;
    private bool _toolBusy;

    public HomeViewModel(
        StagingService staging,
        IFolderPicker folderPicker,
        IAppToolManager toolManager)
    {
        _staging = staging;
        _folderPicker = folderPicker;
        _toolManager = toolManager;

        AddRootFolderCommand = new RelayCommand(_ => _ = AddRootFolderAsync());
        RenameRootCommand = new AsyncRelayCommand(RenameRootAsync, () => SelectedRoot is not null);
        DeleteRootCommand = new AsyncRelayCommand(DeleteRootAsync, () => SelectedRoot is not null);
        SetupMediaEngineCommand = new AsyncRelayCommand(
            SetupMediaEngineAsync,
            () => !_toolBusy && !_toolManager.IsReady);

        LocalizationService.Instance.PropertyChanged += (_, e) =>
        {
            if (e.PropertyName is "Item[]" or nameof(LocalizationService.ActiveCode))
            {
                OnPropertyChanged(nameof(RootSummary));
                OnPropertyChanged(nameof(ToolButtonText));
                OnPropertyChanged(nameof(ToolStatus));
            }
        };
    }

    public ObservableCollection<RootFolder> Roots { get; } = [];

    public RootFolder? SelectedRoot
    {
        get => _selectedRoot;
        set
        {
            if (!SetProperty(ref _selectedRoot, value))
                return;

            RenameRootCommand.RaiseCanExecuteChanged();
            DeleteRootCommand.RaiseCanExecuteChanged();
        }
    }

    public RelayCommand AddRootFolderCommand { get; }
    public AsyncRelayCommand RenameRootCommand { get; }
    public AsyncRelayCommand DeleteRootCommand { get; }
    public AsyncRelayCommand SetupMediaEngineCommand { get; }

    public event Action<RootFolder>? OpenRootRequested;

    public string ToolStatus
    {
        get => _toolStatus;
        private set => SetProperty(ref _toolStatus, value);
    }

    public bool ToolReady => _toolManager.IsReady;

    public string ToolButtonText => ToolReady
        ? LocalizationService.Instance.T("Home.EngineReady")
        : LocalizationService.Instance.T("Home.Setup");

    public string RootSummary =>
        LocalizationService.Instance.T("Library.RootCount", Roots.Count);

    public void Refresh()
    {
        Roots.Clear();
        foreach (var root in _staging.Roots)
            Roots.Add(root);

        if (_selectedRoot is not null &&
            Roots.All(x => x.Id != _selectedRoot.Id))
        {
            SelectedRoot = null;
        }

        ToolStatus = _toolManager.IsReady
            ? LocalizationService.Instance.T("Home.EngineReady")
            : LocalizationService.Instance.T("Home.EngineNeedsSetup");

        OnPropertyChanged(nameof(ToolReady));
        OnPropertyChanged(nameof(ToolButtonText));
        OnPropertyChanged(nameof(RootSummary));
        RenameRootCommand.RaiseCanExecuteChanged();
        DeleteRootCommand.RaiseCanExecuteChanged();
        SetupMediaEngineCommand.RaiseCanExecuteChanged();
    }

    public async Task AddRootFolderAsync()
    {
        var path = _folderPicker.PickFolder();
        if (string.IsNullOrWhiteSpace(path))
            return;

        try
        {
            await _staging.AddRootAsync(path);
            Refresh();
        }
        catch (Exception ex)
        {
            ToolStatus = ex.Message;
        }
    }

    public void Open(RootFolder root) =>
        OpenRootRequested?.Invoke(root);

    private async Task RenameRootAsync()
    {
        if (SelectedRoot is null)
            return;

        var name = await Task.FromResult(
            new WpfTextPromptService().Prompt(
                LocalizationService.Instance.T("Explorer.Rename"),
                LocalizationService.Instance.T("Explorer.RenamePrompt"),
                SelectedRoot.Name));

        if (string.IsNullOrWhiteSpace(name))
            return;

        try
        {
            await _staging.RenameRootAsync(SelectedRoot.Id, name);
            Refresh();
        }
        catch (Exception ex)
        {
            ToolStatus = ex.Message;
        }
    }

    private async Task DeleteRootAsync()
    {
        if (SelectedRoot is null)
            return;

        var message = LocalizationService.Instance.T(
            "Explorer.DeleteFolderPrompt",
            SelectedRoot.Name);

        if (!new WpfTextPromptService().Confirm(
                LocalizationService.Instance.T("Explorer.ConfirmDelete"),
                message))
            return;

        try
        {
            var id = SelectedRoot.Id;
            await _staging.RemoveRootAsync(id);
            SelectedRoot = null;
            Refresh();
        }
        catch (Exception ex)
        {
            ToolStatus = ex.Message;
        }
    }

    private async Task SetupMediaEngineAsync()
    {
        _toolBusy = true;
        ToolStatus = LocalizationService.Instance.T("Home.SettingUpEngine");
        SetupMediaEngineCommand.RaiseCanExecuteChanged();

        try
        {
            var progress = new Progress<string>(message => ToolStatus = message);
            await _toolManager.EnsureReadyAsync(progress);
        }
        catch (Exception ex)
        {
            ToolStatus = ex.Message;
        }
        finally
        {
            _toolBusy = false;
            Refresh();
        }
    }
}
