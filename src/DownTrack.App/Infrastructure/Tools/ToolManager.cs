using System.IO.Compression;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using DownTrack.Application.Services;
using DownTrack.Infrastructure;
using DownTrack.Infrastructure.Localization;

namespace DownTrack.Infrastructure.Tools;

public sealed class ToolManager : IAppToolManager
{
    private const string YtDlpUrl =
        "https://github.com/yt-dlp/yt-dlp/releases/latest/download/yt-dlp.exe";
    private const string YtDlpChecksumsUrl =
        "https://github.com/yt-dlp/yt-dlp/releases/latest/download/SHA2-256SUMS";
    private const string FfmpegUrl =
        "https://www.gyan.dev/ffmpeg/builds/ffmpeg-release-essentials.zip";
    private const string FfmpegChecksumUrl =
        "https://www.gyan.dev/ffmpeg/builds/ffmpeg-release-essentials.zip.sha256";
    private const string DenoUrl =
        "https://github.com/denoland/deno/releases/latest/download/deno-x86_64-pc-windows-msvc.zip";
    private const string DenoChecksumUrl =
        "https://github.com/denoland/deno/releases/latest/download/deno-x86_64-pc-windows-msvc.zip.sha256sum";

    private static readonly HttpClient Client = CreateClient();
    private readonly SemaphoreSlim _gate = new(1, 1);

    public bool IsReady =>
        IsBundledReady ||
        (File.Exists(AppPaths.YtDlpPath) &&
         File.Exists(Path.Combine(AppPaths.ToolsDirectory, "ffmpeg.exe")) &&
         File.Exists(Path.Combine(AppPaths.ToolsDirectory, "ffprobe.exe")) &&
         File.Exists(AppPaths.DenoPath));

    private static bool IsBundledReady =>
        File.Exists(AppPaths.BundledYtDlpPath) &&
        File.Exists(AppPaths.BundledFfmpegPath) &&
        File.Exists(AppPaths.BundledFfprobePath) &&
        File.Exists(AppPaths.BundledDenoPath);

    public string Status =>
        IsReady
            ? LocalizationService.Instance.T("Home.EngineReady")
            : LocalizationService.Instance.T("Home.EngineNeedsSetup");

    public async Task EnsureReadyAsync(
        IProgress<string>? progress = null,
        CancellationToken cancellationToken = default)
    {
        if (IsReady)
            return;

        await _gate.WaitAsync(cancellationToken);
        try
        {
            if (IsReady)
                return;

            Directory.CreateDirectory(AppPaths.ToolsDirectory);

            if (!File.Exists(AppPaths.YtDlpPath))
            {
                progress?.Report(LocalizationService.Instance.T("Engine.DownloadingYtDlp"));
                await DownloadAndVerifyAsync(
                    YtDlpUrl,
                    YtDlpChecksumsUrl,
                    "yt-dlp.exe",
                    cancellationToken);
            }

            var ffmpeg = Path.Combine(AppPaths.ToolsDirectory, "ffmpeg.exe");
            var ffprobe = Path.Combine(AppPaths.ToolsDirectory, "ffprobe.exe");
            if (!File.Exists(ffmpeg) || !File.Exists(ffprobe))
            {
                progress?.Report(LocalizationService.Instance.T("Engine.DownloadingFfmpeg"));
                await DownloadAndExtractFfmpegAsync(progress, cancellationToken);
            }

            if (!File.Exists(AppPaths.DenoPath))
            {
                progress?.Report(LocalizationService.Instance.T("Engine.InstallingRuntime"));
                await DownloadAndExtractDenoAsync(cancellationToken);
            }

            progress?.Report(LocalizationService.Instance.T("Engine.Ready"));
        }
        finally
        {
            _gate.Release();
        }
    }

    public async Task UpdateAsync(
        IProgress<string>? progress = null,
        CancellationToken cancellationToken = default)
    {
        await _gate.WaitAsync(cancellationToken);
        try
        {
            Directory.CreateDirectory(AppPaths.ToolsDirectory);

            progress?.Report(LocalizationService.Instance.T("Engine.UpdatingYtDlp"));
            await DownloadAndVerifyAsync(
                YtDlpUrl,
                YtDlpChecksumsUrl,
                "yt-dlp.exe",
                cancellationToken);

            progress?.Report(LocalizationService.Instance.T("Engine.UpdatingFfmpeg"));
            await DownloadAndExtractFfmpegAsync(progress, cancellationToken);

            progress?.Report(LocalizationService.Instance.T("Engine.UpdatingDeno"));
            await DownloadAndExtractDenoAsync(cancellationToken);

            progress?.Report(LocalizationService.Instance.T("Engine.Updated"));
        }
        finally
        {
            _gate.Release();
        }
    }

    private static HttpClient CreateClient()
    {
        var client = new HttpClient { Timeout = TimeSpan.FromMinutes(15) };
        client.DefaultRequestHeaders.UserAgent.Add(
            new ProductInfoHeaderValue("DownTrack", "0.1"));
        return client;
    }

    private static async Task DownloadAndVerifyAsync(
        string fileUrl,
        string checksumUrl,
        string fileName,
        CancellationToken cancellationToken)
    {
        var tempPath = Path.Combine(AppPaths.ToolsDirectory, fileName + ".download");

        await using (var source = await Client.GetStreamAsync(fileUrl, cancellationToken))
        await using (var destination = File.Create(tempPath))
        {
            await source.CopyToAsync(destination, cancellationToken);
        }

        var checksumText = await Client.GetStringAsync(checksumUrl, cancellationToken);
        var expected = ExtractChecksum(checksumText, fileName)
            ?? throw new InvalidOperationException(LocalizationService.Instance.T("Engine.HashMissingFile", fileName));

        var actual = await ComputeSha256Async(tempPath, cancellationToken);
        if (!string.Equals(expected, actual, StringComparison.OrdinalIgnoreCase))
        {
            File.Delete(tempPath);
            throw new InvalidOperationException(LocalizationService.Instance.T("Engine.HashFailedFile", fileName));
        }

        File.Move(tempPath, Path.Combine(AppPaths.ToolsDirectory, fileName), overwrite: true);
    }

    private static async Task DownloadAndExtractFfmpegAsync(
        IProgress<string>? progress,
        CancellationToken cancellationToken)
    {
        var zipPath = Path.Combine(AppPaths.ToolsDirectory, "ffmpeg-download.zip");
        var extractPath = Path.Combine(AppPaths.ToolsDirectory, "ffmpeg-extract");

        await using (var source = await Client.GetStreamAsync(FfmpegUrl, cancellationToken))
        await using (var destination = File.Create(zipPath))
        {
            await source.CopyToAsync(destination, cancellationToken);
        }

        progress?.Report(LocalizationService.Instance.T("Engine.VerifyingFfmpeg"));

        var checksumText = await Client.GetStringAsync(FfmpegChecksumUrl, cancellationToken);
        var expected = ExtractChecksum(checksumText, Path.GetFileName(FfmpegUrl))
            ?? throw new InvalidOperationException(LocalizationService.Instance.T("Engine.HashMissingFfmpeg"));

        var actual = await ComputeSha256Async(zipPath, cancellationToken);
        if (!string.Equals(expected, actual, StringComparison.OrdinalIgnoreCase))
        {
            File.Delete(zipPath);
            throw new InvalidOperationException(LocalizationService.Instance.T("Engine.HashFailedFfmpeg"));
        }

        if (Directory.Exists(extractPath))
            Directory.Delete(extractPath, recursive: true);

        Directory.CreateDirectory(extractPath);
        ZipFile.ExtractToDirectory(zipPath, extractPath);

        var ffmpeg = Directory
            .EnumerateFiles(extractPath, "ffmpeg.exe", SearchOption.AllDirectories)
            .FirstOrDefault();

        var ffprobe = Directory
            .EnumerateFiles(extractPath, "ffprobe.exe", SearchOption.AllDirectories)
            .FirstOrDefault();

        if (ffmpeg is null || ffprobe is null)
            throw new InvalidOperationException(LocalizationService.Instance.T("Engine.FfmpegMissingExecutables"));

        File.Copy(ffmpeg, Path.Combine(AppPaths.ToolsDirectory, "ffmpeg.exe"), overwrite: true);
        File.Copy(ffprobe, Path.Combine(AppPaths.ToolsDirectory, "ffprobe.exe"), overwrite: true);

        File.Delete(zipPath);
        Directory.Delete(extractPath, recursive: true);
    }

    private static async Task DownloadAndExtractDenoAsync(
        CancellationToken cancellationToken)
    {
        var zipPath = Path.Combine(AppPaths.ToolsDirectory, "deno-download.zip");
        var extractPath = Path.Combine(AppPaths.ToolsDirectory, "deno-extract");

        await using (var source = await Client.GetStreamAsync(DenoUrl, cancellationToken))
        await using (var destination = File.Create(zipPath))
        {
            await source.CopyToAsync(destination, cancellationToken);
        }

        var checksumText = await Client.GetStringAsync(DenoChecksumUrl, cancellationToken);
        var expected = ExtractChecksum(
            checksumText,
            Path.GetFileName(DenoUrl))
            ?? throw new InvalidOperationException(LocalizationService.Instance.T("Engine.HashMissingDeno"));

        var actual = await ComputeSha256Async(zipPath, cancellationToken);
        if (!string.Equals(expected, actual, StringComparison.OrdinalIgnoreCase))
        {
            File.Delete(zipPath);
            throw new InvalidOperationException(LocalizationService.Instance.T("Engine.HashFailedDeno"));
        }

        if (Directory.Exists(extractPath))
            Directory.Delete(extractPath, recursive: true);

        Directory.CreateDirectory(extractPath);
        ZipFile.ExtractToDirectory(zipPath, extractPath);

        var deno = Directory
            .EnumerateFiles(extractPath, "deno.exe", SearchOption.AllDirectories)
            .FirstOrDefault();

        if (deno is null)
            throw new InvalidOperationException(LocalizationService.Instance.T("Engine.DenoMissingExecutable"));

        File.Copy(deno, AppPaths.DenoPath, overwrite: true);

        File.Delete(zipPath);
        Directory.Delete(extractPath, recursive: true);
    }

    private static async Task<string> ComputeSha256Async(
        string path,
        CancellationToken cancellationToken)
    {
        await using var stream = File.OpenRead(path);
        var hash = await SHA256.HashDataAsync(stream, cancellationToken);
        return Convert.ToHexString(hash);
    }

    private static string? ExtractChecksum(string text, string fileName)
    {
        foreach (var line in text.Split('\n', StringSplitOptions.RemoveEmptyEntries))
        {
            if (!line.Contains(fileName, StringComparison.OrdinalIgnoreCase))
                continue;

            var match = Regex.Match(line, @"\b[a-fA-F0-9]{64}\b");
            if (match.Success)
                return match.Value;
        }

        var fallback = Regex.Match(text, @"\b[a-fA-F0-9]{64}\b");
        return fallback.Success ? fallback.Value : null;
    }
}