namespace Audex.Application.Enums.Scheduler
{
    public enum ScheduledTaskExecutionStatus
    {
        Unknown = 0,
        Idle = 1,
        Queued = 2,
        InProgress = 3,
        Done = 4,
        DueDone = 5,
        Failed = 6,
        Cancelled = 7,
        Skipped = 8
    }
}
