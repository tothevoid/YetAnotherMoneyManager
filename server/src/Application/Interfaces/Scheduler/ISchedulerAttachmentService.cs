using System;
using System.Threading.Tasks;
using Audex.Application.DTO.FileStorage;
using Audex.Infrastructure.Entities.Scheduler;

namespace Audex.Application.Interfaces.Scheduler
{
    public interface ISchedulerAttachmentService
    {
        Task<FileStreamDto> GetAttachmentFileStreamAsync(Guid attachmentId);

        Task<ScheduledTaskAttachment> SaveAttachmentAsync(Guid occurrenceId, ScheduledTaskAttachment attachment);
    }
}
