using DownTrack.Core.Enums;
using DownTrack.Core.Models;
using DownTrack.Infrastructure.Storage;
using DownTrack.Infrastructure.Localization;

namespace DownTrack.Application.Services;

public sealed class StagingService(IAppStateStore store, ICommitService commitService) : IStagingService
{
    private readonly IAppStateStore _store = store;
    private readonly ICommitService _commitService = commitService;
    private AppState _state = new();

    public IReadOnlyList<RootFolder> Roots => _state.RootFolders;

    public async Task InitializeAsync()
    {
        _state = await _store.LoadAsync();
    }

    public IReadOnlyList<PendingChange> GetPendingChanges(Guid rootId) =>
        _state.PendingChanges.Where(x => x.RootFolderId == rootId).ToList();

    public IReadOnlyList<VirtualEntry> GetEntries(RootFolder root, string path)
    {
        var currentPath = Normalize(path);

        if (IsDeletedPath(root.Id, currentPath))
            return [];

        var entries = new Dictionary<string, VirtualEntry>(StringComparer.OrdinalIgnoreCase);

        if (Directory.Exists(currentPath))
        {
            foreach (var entryPath in Directory.EnumerateFileSystemEntries(currentPath))
            {
                var isDirectory = Directory.Exists(entryPath);
                entries[Normalize(entryPath)] = new VirtualEntry
                {
                    Name = Path.GetFileName(entryPath),
                    FullPath = Normalize(entryPath),
                    IsDirectory = isDirectory,
                    State = VirtualEntryState.Synced
                };
            }
        }

        foreach (var change in GetPendingChanges(root.Id))
        {
            switch (change.ChangeType)
            {
                case PendingChangeType.CreateFolder:
                    AddPendingEntryIfDirectChild(entries, currentPath, change.TargetPath!, true, change);
                    break;

                case PendingChangeType.Download:
                    AddPendingEntryIfDirectChild(entries, currentPath, change.TargetPath!, false, change);
                    break;

                case PendingChangeType.Rename:
                    if (IsDirectChild(currentPath, change.SourcePath))
                        entries.Remove(Normalize(change.SourcePath!));

                    if (IsDirectChild(currentPath, change.TargetPath))
                        AddPendingEntry(entries, change.TargetPath!, change.IsDirectory, change);
                    break;

                case PendingChangeType.Delete:
                    if (IsDirectChild(currentPath, change.SourcePath))
                        entries.Remove(Normalize(change.SourcePath!));
                    break;
            }
        }

        return entries.Values
            .OrderByDescending(x => x.IsDirectory)
            .ThenBy(x => x.Name, StringComparer.OrdinalIgnoreCase)
            .ToList();
    }

    public async Task AddRootAsync(string path)
    {
        var fullPath = Normalize(path);

        if (!Directory.Exists(fullPath))
            throw new DirectoryNotFoundException(fullPath);

        if (_state.RootFolders.Any(x => PathsEqual(x.Path, fullPath)))
            return;

        _state.RootFolders.Add(new RootFolder
        {
            Name = GetFolderName(fullPath),
            Path = fullPath
        });

        await PersistAsync();
    }

    public async Task RemoveRootAsync(Guid rootId)
    {
        _state.RootFolders.RemoveAll(x => x.Id == rootId);
        _state.PendingChanges.RemoveAll(x => x.RootFolderId == rootId);
        await PersistAsync();
    }

    public async Task StageCreateFolderAsync(RootFolder root, string parentPath, string name)
    {
        var safeName = SanitizeName(name, "New Folder");
        var parent = Normalize(parentPath);
        var target = Normalize(Path.Combine(parent, safeName));

        EnsureInsideRoot(root, target);

        if (GetEntries(root, parent).Any(x => PathsEqual(x.FullPath, target)))
            throw new IOException(LocalizationService.Instance.T("Errors.ItemExists", safeName));

        _state.PendingChanges.Add(new PendingChange
        {
            RootFolderId = root.Id,
            ChangeType = PendingChangeType.CreateFolder,
            TargetPath = target,
            IsDirectory = true
        });

        await PersistAsync();
    }

    public async Task StageRenameAsync(RootFolder root, string sourcePath, string newName, bool isDirectory)
    {
        var source = Normalize(sourcePath);
        EnsureInsideRoot(root, source);

        if (PathsEqual(source, root.Path))
            throw new InvalidOperationException(LocalizationService.Instance.T("Errors.RootCannotRename"));

        var parent = Normalize(Directory.GetParent(source)?.FullName ?? root.Path);
        var safeName = SanitizeName(newName, Path.GetFileName(source));

        if (!isDirectory && string.IsNullOrWhiteSpace(Path.GetExtension(safeName)))
            safeName += Path.GetExtension(source);

        var target = Normalize(Path.Combine(parent, safeName));

        if (PathsEqual(source, target))
            return;

        if (GetEntries(root, parent).Any(x => !PathsEqual(x.FullPath, source) && PathsEqual(x.FullPath, target)))
            throw new IOException($"An item named '{safeName}' already exists.");

        var representedChange = _state.PendingChanges.FirstOrDefault(x =>
            x.RootFolderId == root.Id &&
            x.ChangeType is PendingChangeType.CreateFolder or PendingChangeType.Download &&
            x.TargetPath is not null &&
            PathsEqual(x.TargetPath, source));

        if (representedChange is not null)
        {
            representedChange.TargetPath = target;
            if (representedChange.Media is not null)
                representedChange.Media.Title = Path.GetFileNameWithoutExtension(target);

            RewriteDescendantPaths(root.Id, source, target, representedChange.Id);
        }
        else
        {
            _state.PendingChanges.Add(new PendingChange
            {
                RootFolderId = root.Id,
                ChangeType = PendingChangeType.Rename,
                SourcePath = source,
                TargetPath = target,
                IsDirectory = isDirectory
            });

            RewriteDescendantPaths(root.Id, source, target, null);
        }

        await PersistAsync();
    }

    public async Task StageDeleteAsync(RootFolder root, string sourcePath, bool isDirectory)
    {
        var source = Normalize(sourcePath);
        EnsureInsideRoot(root, source);

        if (PathsEqual(source, root.Path))
            throw new InvalidOperationException(LocalizationService.Instance.T("Errors.RootCannotDelete"));

        var relatedPending = _state.PendingChanges
            .Where(x => x.RootFolderId == root.Id && x.TargetPath is not null && PathsEqual(x.TargetPath, source))
            .ToList();

        if (relatedPending.Count > 0)
        {
            foreach (var change in relatedPending)
                _state.PendingChanges.Remove(change);

            _state.PendingChanges.RemoveAll(x =>
                x.RootFolderId == root.Id &&
                IsDescendantPath(x.SourcePath, source));
        }
        else
        {
            _state.PendingChanges.RemoveAll(x =>
                x.RootFolderId == root.Id &&
                (IsDescendantPath(x.SourcePath, source) || IsDescendantPath(x.TargetPath, source)));

            _state.PendingChanges.Add(new PendingChange
            {
                RootFolderId = root.Id,
                ChangeType = PendingChangeType.Delete,
                SourcePath = source,
                IsDirectory = isDirectory
            });
        }

        await PersistAsync();
    }

    public async Task StageDownloadAsync(RootFolder root, string targetFolder, MediaDownloadSpec spec)
    {
        var folder = Normalize(targetFolder);
        EnsureInsideRoot(root, folder);

        var safeTitle = SanitizeName(spec.Title, "Untitled");
        var requested = Normalize(Path.Combine(folder, safeTitle + spec.Extension));
        var target = GetUniquePath(root, requested);

        spec.Title = Path.GetFileNameWithoutExtension(target);

        _state.PendingChanges.Add(new PendingChange
        {
            RootFolderId = root.Id,
            ChangeType = PendingChangeType.Download,
            TargetPath = target,
            IsDirectory = false,
            Media = spec
        });

        await PersistAsync();
    }

    public async Task CancelChangeAsync(Guid rootId, Guid changeId)
    {
        var change = _state.PendingChanges.FirstOrDefault(x => x.RootFolderId == rootId && x.Id == changeId);
        if (change is null)
            return;

        if (change.ChangeType == PendingChangeType.Rename &&
            change.SourcePath is not null &&
            change.TargetPath is not null)
        {
            RewriteDescendantPaths(rootId, change.TargetPath, change.SourcePath, change.Id);
        }

        _state.PendingChanges.RemoveAll(x => x.Id == changeId);

        if (change.ChangeType == PendingChangeType.CreateFolder && change.TargetPath is not null)
        {
            _state.PendingChanges.RemoveAll(x =>
                x.RootFolderId == rootId &&
                (IsDescendantPath(x.SourcePath, change.TargetPath) ||
                 IsDescendantPath(x.TargetPath, change.TargetPath)));
        }

        await PersistAsync();
    }

    public async Task SaveChangesAsync(
        RootFolder root,
        IProgress<string>? progress = null,
        IProgress<int>? overallProgress = null,
        CancellationToken cancellationToken = default)
    {
        var ordered = GetPendingChanges(root.Id)
            .OrderBy(x => x.ChangeType switch
            {
                PendingChangeType.CreateFolder => 0,
                PendingChangeType.Rename => 1,
                PendingChangeType.Download => 2,
                PendingChangeType.Delete => 3,
                _ => 9
            })
            .ThenBy(x => x.TargetPath ?? x.SourcePath ?? string.Empty, StringComparer.OrdinalIgnoreCase)
            .ToList();

        if (ordered.Count == 0)
        {
            overallProgress?.Report(100);
            return;
        }

        overallProgress?.Report(0);

        for (var index = 0; index < ordered.Count; index++)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var change = ordered[index];

            change.Status = PendingChangeStatus.Processing;
            change.ProgressPercent = 10;
            change.ErrorMessage = null;
            await PersistAsync();

            progress?.Report(DescribeChange(change));

            try
            {
                change.ProgressPercent = 25;
                await PersistAsync();

                await _commitService.ApplyAsync(change, progress, cancellationToken);

                change.ProgressPercent = 100;
                change.Status = PendingChangeStatus.Pending;
                await PersistAsync();

                _state.PendingChanges.RemoveAll(x => x.Id == change.Id);
                await PersistAsync();
            }
            catch (OperationCanceledException)
            {
                change.Status = PendingChangeStatus.Pending;
                change.ProgressPercent = 0;
                await PersistAsync();
                throw;
            }
            catch (Exception ex)
            {
                change.Status = PendingChangeStatus.Error;
                change.ProgressPercent = 100;
                change.ErrorMessage = LocalizationService.Instance.T(
                    "Errors.ActionFailed",
                    ex.Message);
                await PersistAsync();
            }

            overallProgress?.Report(
                (int)Math.Round(
                    (index + 1) * 100d / ordered.Count));
        }
    }

    private static string DescribeChange(PendingChange change)
    {
        var source = Path.GetFileName(change.SourcePath) ?? string.Empty;
        var target = Path.GetFileName(change.TargetPath) ?? string.Empty;
        var media = change.Media?.Title ?? target;

        return change.ChangeType switch
        {
            PendingChangeType.CreateFolder =>
                LocalizationService.Instance.T("Pending.CreateFolder", target),
            PendingChangeType.Rename =>
                LocalizationService.Instance.T("Pending.Rename", source, target),
            PendingChangeType.Delete =>
                LocalizationService.Instance.T("Pending.Delete", source),
            PendingChangeType.Download =>
                LocalizationService.Instance.T("Pending.Download", media),
            _ =>
                LocalizationService.Instance.T("Pending.Generic")
        };
    }

    private async Task PersistAsync() => await _store.SaveAsync(_state);

    private void RewriteDescendantPaths(Guid rootId, string source, string target, Guid? exceptId)
    {
        var sourcePrefix = source.EndsWith(Path.DirectorySeparatorChar)
            ? source
            : source + Path.DirectorySeparatorChar;

        foreach (var change in _state.PendingChanges.Where(x => x.RootFolderId == rootId && x.Id != exceptId))
        {
            change.SourcePath = Rewrite(change.SourcePath, source, sourcePrefix, target);
            change.TargetPath = Rewrite(change.TargetPath, source, sourcePrefix, target);
        }
    }

    private static string? Rewrite(string? value, string source, string sourcePrefix, string target)
    {
        if (value is null)
            return null;

        var normalized = Normalize(value);
        if (PathsEqual(normalized, source))
            return target;

        if (normalized.StartsWith(sourcePrefix, StringComparison.OrdinalIgnoreCase))
            return target + normalized[source.Length..];

        return value;
    }

    private string GetUniquePath(RootFolder root, string requested)
    {
        var currentFolder = Path.GetDirectoryName(requested)!;
        var baseName = Path.GetFileNameWithoutExtension(requested);
        var extension = Path.GetExtension(requested);
        var candidate = requested;
        var index = 2;

        while (GetEntries(root, currentFolder).Any(x => PathsEqual(x.FullPath, candidate)))
            candidate = Normalize(Path.Combine(currentFolder, $"{baseName} ({index++}){extension}"));

        return candidate;
    }

    private static void AddPendingEntryIfDirectChild(
        IDictionary<string, VirtualEntry> entries,
        string currentPath,
        string targetPath,
        bool isDirectory,
        PendingChange change)
    {
        if (IsDirectChild(currentPath, targetPath))
            AddPendingEntry(entries, targetPath, isDirectory, change);
    }

    private static void AddPendingEntry(
        IDictionary<string, VirtualEntry> entries,
        string path,
        bool isDirectory,
        PendingChange change)
    {
        var normalized = Normalize(path);
        entries[normalized] = new VirtualEntry
        {
            Name = Path.GetFileName(normalized),
            FullPath = normalized,
            IsDirectory = isDirectory,
            State = change.Status == PendingChangeStatus.Error
                ? VirtualEntryState.Error
                : VirtualEntryState.Pending,
            PendingChangeId = change.Id
        };
    }

    private bool IsDeletedPath(Guid rootId, string path) =>
        _state.PendingChanges.Any(x =>
            x.RootFolderId == rootId &&
            x.ChangeType == PendingChangeType.Delete &&
            IsDescendantPath(path, x.SourcePath));

    private static bool IsDescendantPath(string? candidate, string? ancestor)
    {
        if (candidate is null || ancestor is null)
            return false;

        var c = Normalize(candidate);
        var a = Normalize(ancestor);

        if (PathsEqual(c, a))
            return true;

        return c.StartsWith(a.EndsWith(Path.DirectorySeparatorChar) ? a : a + Path.DirectorySeparatorChar,
            StringComparison.OrdinalIgnoreCase);
    }

    private static bool IsDirectChild(string parent, string? candidate)
    {
        if (candidate is null)
            return false;

        var parentPath = Normalize(parent);
        var candidatePath = Normalize(candidate);

        return Path.GetDirectoryName(candidatePath) is not null &&
               PathsEqual(Normalize(Path.GetDirectoryName(candidatePath)!), parentPath);
    }

    private static string Normalize(string path) => Path.GetFullPath(path);

    private static bool PathsEqual(string? left, string? right) =>
        left is not null && right is not null &&
        string.Equals(Normalize(left), Normalize(right), StringComparison.OrdinalIgnoreCase);

    private static string SanitizeName(string name, string fallback)
    {
        var value = string.IsNullOrWhiteSpace(name) ? fallback : name.Trim();

        foreach (var invalid in Path.GetInvalidFileNameChars())
            value = value.Replace(invalid, '-');

        value = value.Trim().TrimEnd('.');
        return string.IsNullOrWhiteSpace(value) ? fallback : value;
    }

    private static string GetFolderName(string path)
    {
        var trimmed = path.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
        return Path.GetFileName(trimmed) is { Length: > 0 } name ? name : trimmed;
    }

    private static void EnsureInsideRoot(RootFolder root, string path)
    {
        var rootPath = Normalize(root.Path).TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
        var candidate = Normalize(path).TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);

        if (PathsEqual(rootPath, candidate))
            return;

        if (!candidate.StartsWith(rootPath + Path.DirectorySeparatorChar, StringComparison.OrdinalIgnoreCase))
            throw new InvalidOperationException(LocalizationService.Instance.T("Errors.OutsideRoot"));
    }
}
