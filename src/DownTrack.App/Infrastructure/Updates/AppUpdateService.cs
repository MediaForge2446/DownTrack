using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text.RegularExpressions;

namespace DownTrack.Infrastructure.Updates;

public sealed record AppUpdateInfo(
    bool IsUpdateAvailable,
    string CurrentVersion,
    string LatestVersion,
    string DownloadUrl,
    string Sha256,
    string Message);

public sealed class AppUpdateService
{
    private const string ManifestUrl =
        "https://github.com/MediaForge2446/DownTrack/releases/download/nightly/latest.ini";

    private static readonly HttpClient Client = CreateClient();

    public static AppUpdateService Instance { get; } = new();

    private AppUpdateService()
    {
    }

    public string CurrentVersion =>
        Assembly.GetExecutingAssembly()
            .GetName()
            .Version?
            .ToString(3) ?? "0.1.0";

    public async Task<AppUpdateInfo> CheckAsync(CancellationToken cancellationToken = default)
    {
        var current = CurrentVersion;
        var manifest = await Client.GetStringAsync(ManifestUrl, cancellationToken);

        var latest = ReadValue(manifest, "Version");
        var url = ReadValue(manifest, "SetupUrl");
        var hash = ReadValue(manifest, "SetupSha256");

        if (string.IsNullOrWhiteSpace(latest) ||
            string.IsNullOrWhiteSpace(url))
        {
            throw new InvalidOperationException("The update manifest is invalid.");
        }

        var update = CompareVersions(latest, current) > 0;

        return new AppUpdateInfo(
            update,
            current,
            latest,
            url,
            hash,
            update
                ? $"DownTrack {latest} is available."
                : $"DownTrack {current} is up to date.");
    }

    public async Task InstallAsync(
        AppUpdateInfo update,
        IProgress<string>? progress = null,
        CancellationToken cancellationToken = default)
    {
        if (!update.IsUpdateAvailable)
            return;

        var temp = Path.Combine(
            Path.GetTempPath(),
            $"DownTrack-{update.LatestVersion}.exe");

        await using (var source = await Client.GetStreamAsync(update.DownloadUrl, cancellationToken))
        await using (var destination = File.Create(temp))
        {
            await source.CopyToAsync(destination, cancellationToken);
        }

        if (!string.IsNullOrWhiteSpace(update.Sha256))
        {
            await using var hashStream = File.OpenRead(temp);
            var actual = Convert.ToHexString(
                await System.Security.Cryptography.SHA256.HashDataAsync(
                    hashStream,
                    cancellationToken));

            if (!actual.Equals(update.Sha256, StringComparison.OrdinalIgnoreCase))
                throw new InvalidOperationException("The downloaded update failed SHA-256 verification.");
        }

        progress?.Report("Starting the update…");

        var psi = new ProcessStartInfo
        {
            FileName = temp,
            Arguments = "/VERYSILENT /SUPPRESSMSGBOXES /NORESTART /CLOSEAPPLICATIONS /RESTARTAPPLICATIONS",
            UseShellExecute = true
        };

        Process.Start(psi);
    }

    private static string ReadValue(string content, string key)
    {
        var match = Regex.Match(
            content,
            $"(?im)^\\s*{Regex.Escape(key)}\\s*=\\s*(.+?)\\s*$");

        return match.Success ? match.Groups[1].Value.Trim() : string.Empty;
    }

    private static int CompareVersions(string left, string right)
    {
        var l = ParseVersion(left);
        var r = ParseVersion(right);
        return l.CompareTo(r);
    }

    private static Version ParseVersion(string value)
    {
        var clean = new string(value.Where(c => char.IsDigit(c) || c == '.').ToArray());
        return Version.TryParse(clean, out var version)
            ? version
            : new Version(0, 0, 0);
    }

    private static HttpClient CreateClient()
    {
        var client = new HttpClient { Timeout = TimeSpan.FromMinutes(5) };
        client.DefaultRequestHeaders.UserAgent.Add(
            new ProductInfoHeaderValue("DownTrack", "1.0"));
        return client;
    }
}