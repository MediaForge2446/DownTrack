namespace DownTrack.Infrastructure;

public static class AppPaths
{
    public static string RootDirectory =>
        Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "DownTrack");

    public static string StateFile => Path.Combine(RootDirectory, "state.json");
    public static string StateBackupFile => Path.Combine(RootDirectory, "state.json.bak");
    public static string LanguageFile => Path.Combine(RootDirectory, "language.txt");

    // Installed read-only engine shipped with Setup.exe.
    public static string BundledToolsDirectory =>
        Path.Combine(AppContext.BaseDirectory, "Tools");

    public static string BundledYtDlpPath =>
        Path.Combine(BundledToolsDirectory, "yt-dlp.exe");

    public static string BundledDenoPath =>
        Path.Combine(BundledToolsDirectory, "deno.exe");

    public static string BundledFfmpegPath =>
        Path.Combine(BundledToolsDirectory, "ffmpeg.exe");

    public static string BundledFfprobePath =>
        Path.Combine(BundledToolsDirectory, "ffprobe.exe");

    // Writable per-user fallback for updates/recovery.
    public static string ToolsDirectory =>
        Path.Combine(RootDirectory, "Tools");

    public static string YtDlpPath =>
        Path.Combine(ToolsDirectory, "yt-dlp.exe");

    public static string DenoPath =>
        Path.Combine(ToolsDirectory, "deno.exe");
}