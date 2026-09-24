using DownTrack.Core.Models;

namespace DownTrack.Application.Services;

public interface ICommitService
{
    Task ApplyAsync(
        PendingChange change,
        IProgress<string>? progress = null,
        CancellationToken cancellationToken = default);
}