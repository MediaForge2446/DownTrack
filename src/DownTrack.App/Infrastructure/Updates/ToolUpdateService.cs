using DownTrack.Application.Services;

namespace DownTrack.Infrastructure.Updates;

public sealed class ToolUpdateService
{
    private readonly IAppToolManager _toolManager;

    public ToolUpdateService(IAppToolManager toolManager)
    {
        _toolManager = toolManager;
    }

    public async Task UpdateAsync(
        IProgress<string>? progress = null,
        CancellationToken cancellationToken = default)
    {
        await _toolManager.UpdateAsync(progress, cancellationToken);
    }
}