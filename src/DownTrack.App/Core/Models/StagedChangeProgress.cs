namespace DownTrack.Core.Models;

public sealed record StagedChangeProgress(
    Guid ChangeId,
    int CompletedChanges,
    int TotalChanges,
    double CurrentProgress,
    string StatusText,
    bool IsError = false);