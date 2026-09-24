using DownTrack.Infrastructure.Localization;

namespace DownTrack.Core.Models;

public enum VirtualEntryState
{
    Synced,
    Pending,
    Error
}

public sealed class VirtualEntry
{
    public string Name { get; init; } = string.Empty;
    public string FullPath { get; init; } = string.Empty;
    public bool IsDirectory { get; init; }
    public VirtualEntryState State { get; init; }
    public Guid? PendingChangeId { get; init; }

    public string Kind =>
        IsDirectory
            ? LocalizationService.Instance.T("Explorer.Folder")
            : Path.GetExtension(Name).TrimStart('.').ToUpperInvariant();

    public string StateText => State switch
    {
        VirtualEntryState.Pending => LocalizationService.Instance.T("Explorer.Pending"),
        VirtualEntryState.Error => LocalizationService.Instance.T("Explorer.Error"),
        _ => LocalizationService.Instance.T("Explorer.Synced")
    };
}