using DownTrack.Infrastructure;

namespace DownTrack.Infrastructure.Tools;

public sealed class ToolLocator
{
    public string GetYtDlpPath()
    {
        var path = AppPaths.YtDlpPath;
        if (!File.Exists(path))
        {
            throw new FileNotFoundException(
                "yt-dlp is not installed. Run Media Engine setup from Home.",
                path);
        }

        return path;
    }

    public string GetToolsDirectory()
    {
        var ffmpeg = Path.Combine(AppPaths.ToolsDirectory, "ffmpeg.exe");
        var ffprobe = Path.Combine(AppPaths.ToolsDirectory, "ffprobe.exe");

        if (!File.Exists(ffmpeg) || !File.Exists(ffprobe))
        {
            throw new FileNotFoundException(
                "FFmpeg is not installed correctly. Run Media Engine setup from Home.",
                AppPaths.ToolsDirectory);
        }

        return AppPaths.ToolsDirectory;
    }
}