namespace DownTrack.Infrastructure.Updates;

public enum UpdateStatusKind
{
    Idle,
    Checking,
    Available,
    UpToDate,
    Updating,
    Updated,
    Error
}