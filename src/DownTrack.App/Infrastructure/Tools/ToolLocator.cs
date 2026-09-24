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
        if (File.Exists(AppPaths.YtDlpPath))
            return AppPaths.YtDlpPath;

        if (File.Exists(AppPaths.BundledYtDlpPath))
            return AppPaths.BundledYtDlpPath;

        throw new FileNotFoundException(
            "yt-dlp is not installed. Run Media Engine setup from Home.",
            AppPaths.YtDlpPath);
    }

    public string GetToolsDirectory()
    {
        var localFfmpeg = Path.Combine(AppPaths.ToolsDirectory, "ffmpeg.exe");
        var localFfprobe = Path.Combine(AppPaths.ToolsDirectory, "ffprobe.exe");

        if (File.Exists(localFfmpeg) && File.Exists(localFfprobe) && File.Exists(AppPaths.DenoPath))
            return AppPaths.ToolsDirectory;

        if (HasBundledMediaEngine)
            return AppPaths.BundledToolsDirectory;

        throw new FileNotFoundException(
            "FFmpeg is not installed correctly. Run Media Engine setup from Home.",
            AppPaths.ToolsDirectory);
    }

    public string GetDenoPath()
    {
        if (File.Exists(AppPaths.DenoPath))
            return AppPaths.DenoPath;

        if (File.Exists(AppPaths.BundledDenoPath))
            return AppPaths.BundledDenoPath;

        throw new FileNotFoundException(
            "Deno is not installed correctly. Run Media Engine setup from Home.",
            AppPaths.DenoPath);
    }
}