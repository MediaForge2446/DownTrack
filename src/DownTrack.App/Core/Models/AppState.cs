namespace DownTrack.Core.Models;

public sealed class AppState
{
    public int SchemaVersion { get; set; } = 1;
    public List<RootFolder> RootFolders { get; set; } = [];
    public List<PendingChange> PendingChanges { get; set; } = [];
}
