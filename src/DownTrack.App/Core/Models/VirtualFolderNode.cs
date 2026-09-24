namespace DownTrack.Core.Models;

public sealed class VirtualFolderNode
{
    public string Name { get; init; } = string.Empty;
    public string FullPath { get; init; } = string.Empty;
    public VirtualEntryState State { get; init; }
    public List<VirtualFolderNode> Children { get; init; } = [];
}
