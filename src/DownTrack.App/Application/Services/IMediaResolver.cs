using DownTrack.Core.Models;

namespace DownTrack.Application.Services;

public interface IMediaResolver
{
    Task<IReadOnlyList<MediaDownloadSpec>> ResolveAsync(string url, CancellationToken cancellationToken = default);
}
