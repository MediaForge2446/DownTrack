using DownTrack.Core.Models;

namespace DownTrack.Application.Services;

public interface IMediaResolver
{
    Task<IReadOnlyList<MediaDownloadSpec>> ResolveAsync(
        string url,
        IProgress<string>? progress = null,
        CancellationToken cancellationToken = default);
}