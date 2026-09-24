using DownTrack.Core.Models;

namespace DownTrack.Infrastructure.Storage;

public interface IAppStateStore
{
    Task<AppState> LoadAsync(CancellationToken cancellationToken = default);
    Task SaveAsync(AppState state, CancellationToken cancellationToken = default);
}
