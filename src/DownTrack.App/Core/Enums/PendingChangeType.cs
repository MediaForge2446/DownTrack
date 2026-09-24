namespace DownTrack.Core.Enums;

public enum PendingChangeType
{
    CreateFolder = 0,
    Rename = 1,
    Delete = 2,
    Download = 3
}

public enum PendingChangeStatus
{
    Pending = 0,
    Processing = 1,
    Error = 2
}
