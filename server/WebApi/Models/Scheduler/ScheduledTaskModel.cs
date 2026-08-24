using System;
using MoneyManager.Application.Enums.Scheduler;

namespace MoneyManager.WebApi.Models.Scheduler
{
    public class ScheduledTaskModel
    {
        public string TaskName { get; set; }

        public string DisplayName { get; set; }

        public string Description { get; set; }

        public string CronExpression { get; set; }

        public bool IsEnabled { get; set; }

        public DateTime? NextExecutionUtc { get; set; }

        public DateTime? LastExecutionUtc { get; set; }

        public ScheduledTaskExecutionStatus LastExecutionStatus { get; set; } = ScheduledTaskExecutionStatus.Unknown;

        public long? LastExecutionDurationMs { get; set; }

        public string Category { get; set; }
    }
}
