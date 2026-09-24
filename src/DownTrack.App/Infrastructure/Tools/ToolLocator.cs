using DownTrack.Infrastructure;

namespace DownTrack.Infrastructure.Tools;

public sealed class ToolLocator
{
    public string GetYtDlpPath()
    {
        var path = AppPaths.YtDlpPath;
        if (!File.Exists(path))
            throw new FileNotFoundException(
                "yt-dlp is not installed yet. DownTrack will add Tool Manager support during the bootstrap phase.",
                path);

        return path;
    }
}
