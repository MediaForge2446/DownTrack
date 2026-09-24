namespace DownTrack.Infrastructure;

public static class AppPaths
{
    public static string RootDirectory =>
        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "DownTrack");

    public static string StateFile => Path.Combine(RootDirectory, "state.json");
    public static string StateBackupFile => Path.Combine(RootDirectory, "state.json.bak");
    public static string ToolsDirectory => Path.Combine(RootDirectory, "Tools");
    public static string YtDlpPath => Path.Combine(ToolsDirectory, "yt-dlp.exe");
    public static string DenoPath => Path.Combine(ToolsDirectory, "deno.exe");
}
