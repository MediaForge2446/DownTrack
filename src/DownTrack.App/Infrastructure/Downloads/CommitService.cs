using DownTrack.Application.Services;
using DownTrack.Core.Enums;
using DownTrack.Core.Models;

namespace DownTrack.Infrastructure.Downloads;

public sealed class CommitService(YtDlpDownloader downloader) : ICommitService
{
    private readonly YtDlpDownloader _downloader = downloader;

    public async Task ApplyAsync(PendingChange change, CancellationToken cancellationToken = default)
    {
        switch (change.ChangeType)
        {
            case PendingChangeType.CreateFolder:
                Directory.CreateDirectory(change.TargetPath
                    ?? throw new InvalidOperationException("Missing target path."));
                break;

            case PendingChangeType.Rename:
                ApplyRename(change);
                break;

            case PendingChangeType.Delete:
                ApplyDelete(change);
                break;

            case PendingChangeType.Download:
                if (change.Media is null || change.TargetPath is null)
                    throw new InvalidOperationException("Download change is missing media information.");

                await _downloader.DownloadAsync(change.Media, change.TargetPath, cancellationToken);
                break;

            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private static void ApplyRename(PendingChange change)
    {
        if (change.SourcePath is null || change.TargetPath is null)
            throw new InvalidOperationException("Rename change is incomplete.");

        if (File.Exists(change.TargetPath) || Directory.Exists(change.TargetPath))
            throw new IOException($"Target already exists: {change.TargetPath}");

        if (change.IsDirectory)
            Directory.Move(change.SourcePath, change.TargetPath);
        else
            File.Move(change.SourcePath, change.TargetPath);
    }

    private static void ApplyDelete(PendingChange change)
    {
        var path = change.SourcePath
            ?? throw new InvalidOperationException("Delete change is incomplete.");

        if (change.IsDirectory)
        {
            if (Directory.Exists(path))
                Directory.Delete(path, recursive: true);
        }
        else if (File.Exists(path))
        {
            File.Delete(path);
        }
    }
}
