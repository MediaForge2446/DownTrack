namespace DownTrack.Application.Services;

public interface IAppToolManager
{
    bool IsReady { get; }
    string Status { get; }

    Task EnsureReadyAsync(IProgress<string>? progress = null, CancellationToken cancellationToken = default);
}
