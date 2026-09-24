using DownTrack.Application.Services;
using DownTrack.Core.Enums;
using DownTrack.Core.Models;
using DownTrack.Infrastructure.Localization;

namespace DownTrack.ViewModels;

public sealed class PendingChangeItemViewModel : ObservableObject
{
    private double _progress;
    private string _statusText = string.Empty;
    private bool _isError;

    public PendingChangeItemViewModel(PendingChange change)
    {
        Change = change;
        _progress = change.Status == PendingChangeStatus.Error ? 100 : 0;
        _isError = change.Status == PendingChangeStatus.Error;
        _statusText = BuildStatusText(change);
    }

    public PendingChange Change { get; }

    public string Description =>
        Change.ChangeType switch
        {
            PendingChangeType.CreateFolder =>
                LocalizationService.Instance.T("Explorer.PendingCreate", Path.GetFileName(Change.TargetPath)),
            PendingChangeType.Rename =>
                LocalizationService.Instance.T("Explorer.PendingRename", Path.GetFileName(Change.TargetPath)),
            PendingChangeType.Delete =>
                LocalizationService.Instance.T("Explorer.PendingDelete", Path.GetFileName(Change.SourcePath)),
            PendingChangeType.Download =>
                Change.Media?.Title ?? Path.GetFileName(Change.TargetPath) ?? LocalizationService.Instance.T("Explorer.AddMedia"),
            _ => LocalizationService.Instance.T("Pending.Change")
        };

    public double Progress
    {
        get => _progress;
        private set
        {
            if (Math.Abs(_progress - value) < 0.1)
                return;

            _progress = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(ProgressText));
        }
    }

    public string ProgressText => $"{Progress:0}%";

    public string StatusText
    {
        get => _statusText;
        private set => SetProperty(ref _statusText, value);
    }

    public bool IsError
    {
        get => _isError;
        private set => SetProperty(ref _isError, value);
    }

    public bool IsComplete => Progress >= 100 && !IsError;

    public void Apply(StagedChangeProgress update)
    {
        Progress = Math.Clamp(update.CurrentProgress, 0, 100);
        StatusText = update.StatusText;
        IsError = update.IsError;
        OnPropertyChanged(nameof(IsComplete));
    }

    public void MarkCompleted()
    {
        Progress = 100;
        IsError = false;
        StatusText = LocalizationService.Instance.T("Pending.Completed");
        OnPropertyChanged(nameof(IsComplete));
    }

    public void MarkError(string message)
    {
        Progress = 100;
        IsError = true;
        StatusText = message;
        OnPropertyChanged(nameof(IsComplete));
    }

    private static string BuildStatusText(PendingChange change)
    {
        return change.Status switch
        {
            PendingChangeStatus.Processing =>
                LocalizationService.Instance.T("Pending.Processing"),
            PendingChangeStatus.Error =>
                change.ErrorMessage ?? LocalizationService.Instance.T("Pending.Error"),
            _ => LocalizationService.Instance.T("Pending.Waiting")
        };
    }
}