using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MoneyManager.Application.DTO.FileStorage;
using MoneyManager.Application.Interfaces.FileStorage;
using MoneyManager.Application.Interfaces.Scheduler;
using MoneyManager.Infrastructure.Entities.Scheduler;
using MoneyManager.Infrastructure.Interfaces.Database;

namespace MoneyManager.Application.Services.Scheduler
{
    public class SchedulerAttachmentService : ISchedulerAttachmentService
    {
        private readonly IUnitOfWork _db;
        private readonly IRepository<ScheduledTaskAttachment> _attachmentRepo;
        private readonly IFileStorageService _fileStorageService;
        private readonly ILogger<SchedulerAttachmentService> _logger;

        public SchedulerAttachmentService(
            IUnitOfWork unitOfWork,
            IFileStorageService fileStorageService,
            ILogger<SchedulerAttachmentService> logger)
        {
            _db = unitOfWork;
            _attachmentRepo = unitOfWork.CreateRepository<ScheduledTaskAttachment>();
            _fileStorageService = fileStorageService;
            _logger = logger;
        }

        public async Task<FileStreamDto> GetAttachmentFileStreamAsync(Guid attachmentId)
        {
            var attachment = await _attachmentRepo.GetByIdAsync(attachmentId, disableTracking: true);

            if (attachment == null)
            {
                return null;
            }

            try
            {
                var fileStreamDto = await _fileStorageService.GetFileStreamAsync(attachment.BucketName, attachment.StoragePath);
                if (fileStreamDto?.Stream == null)
                {
                    return null;
                }

                return fileStreamDto;
            }
            catch (Exception exception)
            {
                _logger.LogWarning(exception, "Failed to download attachment file {StoragePath} from storage", attachment.StoragePath);
                return null;
            }
        }

        public async Task<ScheduledTaskAttachment> SaveAttachmentAsync(Guid occurrenceId, ScheduledTaskAttachment attachment)
        {
            if (attachment == null || occurrenceId == Guid.Empty)
            {
                return null;
            }

            try
            {
                attachment.OccurrenceId = occurrenceId;
                if (attachment.Id == Guid.Empty)
                {
                    attachment.Id = Guid.NewGuid();
                }

                await _attachmentRepo.AddAsync(attachment);
                await _db.CommitAsync();
            }
            catch (Exception exception)
            {
                _logger.LogWarning(exception, "Failed to save attachment {FileName} for occurrence {OccurrenceId}", attachment.FileName, occurrenceId);
                return null;
            }

            return attachment;
        }
    }
}
