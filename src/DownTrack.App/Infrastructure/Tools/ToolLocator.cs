using DownTrack.Infrastructure;

namespace DownTrack.Infrastructure.Tools;

public sealed class ToolLocator
{
    public bool HasBundledMediaEngine =>
        File.Exists(AppPaths.BundledYtDlpPath) &&
        File.Exists(AppPaths.BundledFfmpegPath) &&
        File.Exists(AppPaths.BundledFfprobePath) &&
        File.Exists(AppPaths.BundledDenoPath);

    public string GetYtDlpPath()
    {
        if (HasBundledMediaEngine)
            return AppPaths.BundledYtDlpPath;

        if (File.Exists(AppPaths.YtDlpPath))
            return AppPaths.YtDlpPath;

        throw new FileNotFoundException(
            "yt-dlp is not installed. Run Media Engine setup from Home.",
            AppPaths.YtDlpPath);
    }

    public string GetToolsDirectory()
    {
        if (HasBundledMediaEngine)
            return AppPaths.BundledToolsDirectory;

        var ffmpeg = Path.Combine(AppPaths.ToolsDirectory, "ffmpeg.exe");
        var ffprobe = Path.Combine(AppPaths.ToolsDirectory, "ffprobe.exe");

        if (File.Exists(ffmpeg) && File.Exists(ffprobe))
            return AppPaths.ToolsDirectory;

        throw new FileNotFoundException(
            "FFmpeg is not installed correctly. Run Media Engine setup from Home.",
            AppPaths.ToolsDirectory);
    }

    public string GetDenoPath()
    {
        if (HasBundledMediaEngine)
            return AppPaths.BundledDenoPath;

        if (File.Exists(AppPaths.DenoPath))
            return AppPaths.DenoPath;

        throw new FileNotFoundException(
            "Deno is not installed correctly. Run Media Engine setup from Home.",
            AppPaths.DenoPath);
    }
}