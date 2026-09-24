using DownTrack.Core.Enums;

namespace DownTrack.Core.Models;

public sealed class PendingChange
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid RootFolderId { get; set; }
    public PendingChangeType ChangeType { get; set; }
    public PendingChangeStatus Status { get; set; } = PendingChangeStatus.Pending;
    public string? SourcePath { get; set; }
    public string? TargetPath { get; set; }
    public bool IsDirectory { get; set; }
    public MediaDownloadSpec? Media { get; set; }
    public string? ErrorMessage { get; set; }
    public int ProgressPercent { get; set; }

    public string Description =>
        ChangeType switch
        {
            PendingChangeType.CreateFolder => $"Create folder: {Path.GetFileName(TargetPath)}",
            PendingChangeType.Rename => $"Rename: {Path.GetFileName(SourcePath)} → {Path.GetFileName(TargetPath)}",
            PendingChangeType.Delete => $"Delete: {Path.GetFileName(SourcePath)}",
            PendingChangeType.Download => $"Download: {Media?.Title ?? Path.GetFileName(TargetPath)}",
            _ => "Pending change"
        };
}
