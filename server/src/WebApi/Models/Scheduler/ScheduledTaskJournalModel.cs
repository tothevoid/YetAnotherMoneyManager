using System;
using System.Collections.Generic;
using Audex.Application.Enums.Scheduler;

namespace Audex.WebApi.Models.Scheduler
{
    public class ScheduledTaskJournalModel
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

        public List<ScheduledTaskAttachmentModel> Attachments { get; set; } = new List<ScheduledTaskAttachmentModel>();
    }
}
