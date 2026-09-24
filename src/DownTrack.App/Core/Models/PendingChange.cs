using System.ComponentModel;
using System.Runtime.CompilerServices;
using DownTrack.Core.Enums;

namespace DownTrack.Core.Models;

public sealed class PendingChange : INotifyPropertyChanged
{
    private PendingChangeStatus _status = PendingChangeStatus.Pending;
    private string? _errorMessage;
    private int _progressPercent;

    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid RootFolderId { get; set; }
    public PendingChangeType ChangeType { get; set; }
    public PendingChangeStatus Status
    {
        get => _status;
        set => SetField(ref _status, value);
    }

    public string? SourcePath { get; set; }
    public string? TargetPath { get; set; }
    public bool IsDirectory { get; set; }
    public MediaDownloadSpec? Media { get; set; }

    public string? ErrorMessage
    {
        get => _errorMessage;
        set => SetField(ref _errorMessage, value);
    }

    public int ProgressPercent
    {
        get => _progressPercent;
        set => SetField(ref _progressPercent, Math.Clamp(value, 0, 100));
    }

    public string Description =>
        ChangeType switch
        {
            PendingChangeType.CreateFolder => $"Create folder: {Path.GetFileName(TargetPath)}",
            PendingChangeType.Rename => $"Rename: {Path.GetFileName(SourcePath)} → {Path.GetFileName(TargetPath)}",
            PendingChangeType.Delete => $"Delete: {Path.GetFileName(SourcePath)}",
            PendingChangeType.Download => $"Download: {Media?.Title ?? Path.GetFileName(TargetPath)}",
            _ => "Pending change"
        };

    public event PropertyChangedEventHandler? PropertyChanged;

    private void SetField<T>(
        ref T field,
        T value,
        [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value))
            return;

        field = value;
        PropertyChanged?.Invoke(this, new(propertyName));
    }
}