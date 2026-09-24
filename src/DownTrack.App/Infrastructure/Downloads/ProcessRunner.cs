using System.Diagnostics;
using System.Text;
using DownTrack.Application.Services;

namespace DownTrack.Infrastructure.Downloads;

public sealed class ProcessRunner : IProcessRunner
{
    public async Task<ProcessResult> RunAsync(
        string executablePath,
        string arguments,
        string? workingDirectory = null,
        CancellationToken cancellationToken = default)
    {
        using var process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = executablePath,
                Arguments = arguments,
                WorkingDirectory = workingDirectory ?? AppContext.BaseDirectory,
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                StandardOutputEncoding = Encoding.UTF8,
                StandardErrorEncoding = Encoding.UTF8
            }
        };

        if (!process.Start())
            throw new InvalidOperationException($"Could not start process: {executablePath}");

        var stdout = new StringBuilder();
        var stderr = new StringBuilder();

        var outputTask = ReadOutputAsync(process.StandardOutput, stdout);
        var errorTask = ReadOutputAsync(process.StandardError, stderr);

        try
        {
            await process.WaitForExitAsync(cancellationToken);
            await Task.WhenAll(outputTask, errorTask);
        }
        catch (OperationCanceledException)
        {
            try
            {
                if (!process.HasExited)
                    process.Kill(entireProcessTree: true);
            }
            catch
            {
                // Best effort process cleanup.
            }

            try
            {
                await Task.WhenAll(outputTask, errorTask);
            }
            catch
            {
                // The process may have been terminated while output was draining.
            }

            throw;
        }

        return new ProcessResult(
            process.ExitCode,
            stdout.ToString(),
            stderr.ToString());
    }

    private static async Task ReadOutputAsync(
        StreamReader reader,
        StringBuilder destination)
    {
        while (await reader.ReadLineAsync() is { } line)
            destination.AppendLine(line);
    }
}