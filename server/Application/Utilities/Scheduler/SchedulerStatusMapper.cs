using MoneyManager.Application.Enums.Scheduler;
using TickerQ.Utilities.Enums;

namespace MoneyManager.Application.Utilities.Scheduler
{
    public static class SchedulerStatusMapper
    {
        public static ScheduledTaskExecutionStatus ToExecutionStatus(TickerStatus status)
        {
            return status switch
            {
                TickerStatus.Done => ScheduledTaskExecutionStatus.Success,
                TickerStatus.Failed => ScheduledTaskExecutionStatus.Failed,
                _ => ScheduledTaskExecutionStatus.Running
            };
        }

        public static TickerStatus ToTickerStatus(ScheduledTaskExecutionStatus status)
        {
            return status switch
            {
                ScheduledTaskExecutionStatus.Success => TickerStatus.Done,
                ScheduledTaskExecutionStatus.Failed => TickerStatus.Failed,
                _ => TickerStatus.Queued
            };
        }
    }
}
