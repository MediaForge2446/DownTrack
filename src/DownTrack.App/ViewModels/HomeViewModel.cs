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
        SetupMediaEngineCommand = new AsyncRelayCommand(
            SetupMediaEngineAsync,
            () => !_toolBusy && !_toolManager.IsReady);

        LocalizationService.Instance.PropertyChanged += (_, e) =>
        {
            if (e.PropertyName is "Item[]" or nameof(LocalizationService.ActiveCode))
            {
                ToolStatus = _toolManager.IsReady
                    ? LocalizationService.Instance.T("Home.EngineReady")
                    : LocalizationService.Instance.T("Home.EngineNeedsSetup");
                OnPropertyChanged(nameof(ToolButtonText));
                OnPropertyChanged(nameof(RootSummary));
            }
        };
    }

    public ObservableCollection<RootFolder> Roots { get; } = [];
    public RelayCommand AddRootFolderCommand { get; }
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
        LocalizationService.Instance.T("Home.LibrarySubtitle", Roots.Count);

    public void Refresh()
    {
        Roots.Clear();
        foreach (var root in _staging.Roots)
            Roots.Add(root);

        ToolStatus = _toolManager.IsReady
            ? LocalizationService.Instance.T("Home.EngineReady")
            : LocalizationService.Instance.T("Home.EngineNeedsSetup");
        OnPropertyChanged(nameof(ToolReady));
        OnPropertyChanged(nameof(ToolButtonText));
        OnPropertyChanged(nameof(RootSummary));
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

    public void Open(RootFolder root) => OpenRootRequested?.Invoke(root);

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
