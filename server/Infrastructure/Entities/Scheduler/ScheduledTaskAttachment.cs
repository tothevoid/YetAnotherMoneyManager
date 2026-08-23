using System;
using MoneyManager.Shared.Entities;
using TickerQ.Utilities.Entities;

namespace MoneyManager.Infrastructure.Entities.Scheduler
{
    public class ScheduledTaskAttachment : BaseEntity
    {
        public Guid OccurrenceId { get; set; }

        public CronTickerOccurrenceEntity<ScheduledCronTicker> Occurrence { get; set; }

        public string FileName { get; set; }

        public string StoragePath { get; set; }

        public string ContentType { get; set; }

        public long FileSizeBytes { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
