using DownTrack.Core.Models;

namespace DownTrack.Application.Services;

public interface IStagingService
{
    IReadOnlyList<RootFolder> Roots { get; }
    IReadOnlyList<PendingChange> GetPendingChanges(Guid rootId);
    IReadOnlyList<VirtualEntry> GetEntries(RootFolder root, string path);

    Task InitializeAsync();
    Task AddRootAsync(string path);
    Task RemoveRootAsync(Guid rootId);
    Task RenameRootAsync(Guid rootId, string name);

    Task StageCreateFolderAsync(RootFolder root, string parentPath, string name);
    Task StageRenameAsync(RootFolder root, string sourcePath, string newName, bool isDirectory);
    Task StageDeleteAsync(RootFolder root, string sourcePath, bool isDirectory);
    Task StageDownloadAsync(RootFolder root, string targetFolder, MediaDownloadSpec spec);

    Task CancelChangeAsync(Guid rootId, Guid changeId);
    Task SaveChangesAsync(RootFolder root, IProgress<StagedChangeProgress>? progress = null, CancellationToken cancellationToken = default);
}
