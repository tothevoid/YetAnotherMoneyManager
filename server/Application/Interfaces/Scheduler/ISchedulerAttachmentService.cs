using System;
using System.Threading.Tasks;
using MoneyManager.Application.DTO.FileStorage;
using MoneyManager.Infrastructure.Entities.Scheduler;

namespace MoneyManager.Application.Interfaces.Scheduler
{
    public interface ISchedulerAttachmentService
    {
        Task<FileStreamDto> GetAttachmentFileStreamAsync(Guid attachmentId);

        Task<ScheduledTaskAttachment> SaveAttachmentAsync(Guid occurrenceId, ScheduledTaskAttachment attachment);
    }
}
