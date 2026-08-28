using System;
using System.Collections.Generic;
using MoneyManager.Application.Enums.Scheduler;

namespace MoneyManager.Application.DTO.Scheduler
{
    public class ScheduledTaskJournalDto
    {
        public Guid Id { get; set; }

        public string TaskName { get; set; }

        public string DisplayName { get; set; }

        public DateTime ExecutedAtUtc { get; set; }

        public long DurationMs { get; set; }

        public ScheduledTaskExecutionStatus Status { get; set; }

        public ScheduledTaskTriggerSource TriggerSource { get; set; }

        public string LogMessage { get; set; }

        public string ErrorMessage { get; set; }

        public List<ScheduledTaskAttachmentDto> Attachments { get; set; } = new List<ScheduledTaskAttachmentDto>();
    }
}
