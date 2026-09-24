using DownTrack.Application.Services;
using DownTrack.Core.Enums;
using DownTrack.Core.Models;
using DownTrack.Infrastructure.Localization;

namespace DownTrack.Infrastructure.Downloads;

public sealed class CommitService(
    YtDlpDownloader downloader,
    IAppToolManager toolManager) : ICommitService
{
    private readonly YtDlpDownloader _downloader = downloader;
    private readonly IAppToolManager _toolManager = toolManager;

    public async Task ApplyAsync(
        PendingChange change,
        IProgress<string>? progress = null,
        CancellationToken cancellationToken = default)
    {
        switch (change.ChangeType)
        {
            case PendingChangeType.CreateFolder:
                Directory.CreateDirectory(change.TargetPath
                    ?? throw new InvalidOperationException(LocalizationService.Instance.T("Error.MissingTargetPath")));
                break;

            case PendingChangeType.Rename:
                ApplyRename(change);
                break;

            case PendingChangeType.Delete:
                ApplyDelete(change);
                break;

            case PendingChangeType.Download:
                if (change.Media is null || change.TargetPath is null)
                    throw new InvalidOperationException(LocalizationService.Instance.T("Error.MissingMedia"));

                progress?.Report(LocalizationService.Instance.T("Download.PreparingEngine"));
                await _toolManager.EnsureReadyAsync(progress, cancellationToken);

                progress?.Report(LocalizationService.Instance.T("Download.Downloading", change.Media.Title));
                await _downloader.DownloadAsync(change.Media, change.TargetPath, cancellationToken);
                break;

            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private static void ApplyRename(PendingChange change)
    {
        if (change.SourcePath is null || change.TargetPath is null)
            throw new InvalidOperationException(LocalizationService.Instance.T("Error.IncompleteRename"));

        if (File.Exists(change.TargetPath) || Directory.Exists(change.TargetPath))
            throw new IOException(LocalizationService.Instance.T("Error.TargetExists", change.TargetPath));

        if (change.IsDirectory)
            Directory.Move(change.SourcePath, change.TargetPath);
        else
            File.Move(change.SourcePath, change.TargetPath);
    }

    private static void ApplyDelete(PendingChange change)
    {
        var path = change.SourcePath
            ?? throw new InvalidOperationException(LocalizationService.Instance.T("Error.IncompleteDelete"));

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