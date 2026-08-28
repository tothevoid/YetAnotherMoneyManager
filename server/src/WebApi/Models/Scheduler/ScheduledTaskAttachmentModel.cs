using System;

namespace Audex.WebApi.Models.Scheduler
{
    public class ScheduledTaskAttachmentModel
    {
        public Guid Id { get; set; }

        public Guid OccurrenceId { get; set; }

        public string FileName { get; set; }

        public string BucketName { get; set; }

        public string StoragePath { get; set; }

        public string ContentType { get; set; }

        public long FileSizeBytes { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
