using Audex.Application.Enums.Scheduler;
using TickerQ.Utilities.Enums;

namespace Audex.Application.Utilities.Scheduler
{
    public static class SchedulerStatusMapper
    {
        public static ScheduledTaskExecutionStatus ToExecutionStatus(TickerStatus status)
        {
            return status switch
            {
                TickerStatus.Idle => ScheduledTaskExecutionStatus.Idle,
                TickerStatus.Queued => ScheduledTaskExecutionStatus.Queued,
                TickerStatus.InProgress => ScheduledTaskExecutionStatus.InProgress,
                TickerStatus.Done => ScheduledTaskExecutionStatus.Done,
                TickerStatus.DueDone => ScheduledTaskExecutionStatus.DueDone,
                TickerStatus.Failed => ScheduledTaskExecutionStatus.Failed,
                TickerStatus.Cancelled => ScheduledTaskExecutionStatus.Cancelled,
                TickerStatus.Skipped => ScheduledTaskExecutionStatus.Skipped,
                _ => ScheduledTaskExecutionStatus.Unknown
            };
        }

        public static TickerStatus ToTickerStatus(ScheduledTaskExecutionStatus status)
        {
            return status switch
            {
                ScheduledTaskExecutionStatus.Idle => TickerStatus.Idle,
                ScheduledTaskExecutionStatus.Queued => TickerStatus.Queued,
                ScheduledTaskExecutionStatus.InProgress => TickerStatus.InProgress,
                ScheduledTaskExecutionStatus.Done => TickerStatus.Done,
                ScheduledTaskExecutionStatus.DueDone => TickerStatus.DueDone,
                ScheduledTaskExecutionStatus.Failed => TickerStatus.Failed,
                ScheduledTaskExecutionStatus.Cancelled => TickerStatus.Cancelled,
                ScheduledTaskExecutionStatus.Skipped => TickerStatus.Skipped,
                _ => TickerStatus.Queued
            };
        }
    }
}
