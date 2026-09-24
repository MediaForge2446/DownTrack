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

        var outputTask = Task.Run(async () =>
        {
            while (await process.StandardOutput.ReadLineAsync() is { } line)
                stdout.AppendLine(line);
        }, cancellationToken);

        var errorTask = Task.Run(async () =>
        {
            while (await process.StandardError.ReadLineAsync() is { } line)
                stderr.AppendLine(line);
        }, cancellationToken);

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

            throw;
        }

        return new ProcessResult(process.ExitCode, stdout.ToString(), stderr.ToString());
    }
}
