using System;

namespace Audex.Application.DTO.Scheduler
{
    public class ScheduledTaskAttachmentDto
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
