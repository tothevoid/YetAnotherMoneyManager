using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MoneyManager.Application.DTO.Common;
using MoneyManager.Application.DTO.Scheduler;
using MoneyManager.Application.Enums.Scheduler;
using MoneyManager.Application.Interfaces.Scheduler;
using MoneyManager.Application.Utilities.Scheduler;
using MoneyManager.Infrastructure.Entities.Scheduler;
using MoneyManager.Infrastructure.Interfaces.Database;
using MoneyManager.Infrastructure.Queries;
using TickerQ.Utilities.Entities;
using TickerQ.Utilities.Enums;

namespace MoneyManager.Application.Services.Scheduler
{
    public class SchedulerJournalService : ISchedulerJournalService
    {
        private readonly IUnitOfWork _db;
        private readonly IRepository<ScheduledCronTicker> _tickerRepo;
        private readonly IRepository<CronTickerOccurrenceEntity<ScheduledCronTicker>> _occurrenceRepo;
        private readonly IRepository<ScheduledTaskAttachment> _attachmentRepo;
        private readonly IScheduledJobRegistry _jobRegistry;
        private readonly ISchedulerAttachmentService _attachmentService;
        private readonly ILogger<SchedulerJournalService> _logger;

        public SchedulerJournalService(
            IUnitOfWork unitOfWork,
            IScheduledJobRegistry jobRegistry,
            ISchedulerAttachmentService attachmentService,
            ILogger<SchedulerJournalService> logger)
        {
            _db = unitOfWork;
            _tickerRepo = unitOfWork.CreateRepository<ScheduledCronTicker>();
            _occurrenceRepo = unitOfWork.CreateRepository<CronTickerOccurrenceEntity<ScheduledCronTicker>>();
            _attachmentRepo = unitOfWork.CreateRepository<ScheduledTaskAttachment>();
            _jobRegistry = jobRegistry;
            _attachmentService = attachmentService;
            _logger = logger;
        }

        public async Task<IEnumerable<ScheduledTaskJournalDto>> GetJournalAsync(
            int pageIndex = 1,
            int recordsQuantity = 20,
            string taskName = null,
            ScheduledTaskExecutionStatus? status = null,
            ScheduledTaskTriggerSource? triggerSource = null)
        {
            var builder = new ComplexQueryBuilder<CronTickerOccurrenceEntity<ScheduledCronTicker>>()
                .AddJoins(query => query.Include(occurrence => occurrence.CronTicker))
                .AddFilter(GetFilter(taskName, status))
                .AddPagination(pageIndex, recordsQuantity, occurrence => occurrence.ExecutionTime, isDescending: true)
                .DisableTracking();

            var occurrences = await _occurrenceRepo.GetAllAsync(builder.GetQuery());
            var occurrenceList = occurrences.ToList();
            var occurrenceIds = occurrenceList.Select(occurrence => occurrence.Id).ToHashSet();

            var attachments = await _attachmentRepo.GetAllAsync(
                filter: attachment => occurrenceIds.Contains(attachment.OccurrenceId),
                disableTracking: true);

            var attachmentMap = attachments
                .GroupBy(attachment => attachment.OccurrenceId)
                .ToDictionary(group => group.Key, group => group.Select(MapAttachmentDto).ToList());

            return occurrenceList.Select(occurrence =>
            {
                var functionName = occurrence.CronTicker?.Function ?? "ScheduledTask";
                var displayName = _jobRegistry.TryGetDescriptor(functionName, out var jobDescriptor)
                    ? jobDescriptor.DisplayName
                    : functionName;

                return new ScheduledTaskJournalDto
                {
                    Id = occurrence.Id,
                    TaskName = functionName,
                    DisplayName = displayName,
                    ExecutedAtUtc = occurrence.ExecutedAt ?? occurrence.ExecutionTime,
                    DurationMs = occurrence.ElapsedTime,
                    Status = SchedulerStatusMapper.ToExecutionStatus(occurrence.Status),
                    TriggerSource = triggerSource ?? ScheduledTaskTriggerSource.Scheduled,
                    LogMessage = null,
                    ErrorMessage = occurrence.ExceptionMessage,
                    Attachments = attachmentMap.TryGetValue(occurrence.Id, out var taskAttachments) ? taskAttachments : new List<ScheduledTaskAttachmentDto>()
                };
            }).ToList();
        }

        public async Task<PaginationConfigDto> GetJournalPaginationAsync(
            string taskName = null,
            ScheduledTaskExecutionStatus? status = null,
            ScheduledTaskTriggerSource? triggerSource = null)
        {
            var recordsQuantity = await _occurrenceRepo.GetCountAsync(GetFilter(taskName, status));

            return new PaginationConfigDto
            {
                PageSize = 15,
                RecordsQuantity = recordsQuantity
            };
        }

        public async Task<ScheduledTaskAttachment> RecordExecutionAsync(
            string taskName,
            ScheduledTaskExecutionStatus status,
            long durationMs,
            ScheduledTaskTriggerSource triggerSource = ScheduledTaskTriggerSource.Scheduled,
            string logMessage = null,
            string errorMessage = null,
            ScheduledTaskAttachment attachment = null)
        {
            var occurrenceId = attachment?.OccurrenceId ?? Guid.NewGuid();
            var executionTime = DateTime.UtcNow;

            try
            {
                var ticker = await _tickerRepo.FindAsync(existingTicker => existingTicker.Function == taskName);

                if (ticker == null)
                {
                    _logger.LogWarning("Cannot record execution: ticker for task '{TaskName}' is not registered by user in DB", taskName);
                    return null;
                }

                var occurrence = new CronTickerOccurrenceEntity<ScheduledCronTicker>
                {
                    Id = occurrenceId,
                    CronTickerId = ticker.Id,
                    ExecutionTime = executionTime,
                    ExecutedAt = executionTime,
                    ElapsedTime = durationMs,
                    Status = SchedulerStatusMapper.ToTickerStatus(status),
                    ExceptionMessage = errorMessage
                };

                await _occurrenceRepo.AddAsync(occurrence);
                await _db.CommitAsync();

                if (attachment != null)
                {
                    await _attachmentService.SaveAttachmentAsync(occurrenceId, attachment);
                }
            }
            catch (Exception exception)
            {
                _logger.LogWarning(exception, "Failed to persist execution record in DB for task {TaskName}", taskName);
            }

            return attachment;
        }

        private static Expression<Func<CronTickerOccurrenceEntity<ScheduledCronTicker>, bool>> GetFilter(
            string taskName,
            ScheduledTaskExecutionStatus? status)
        {
            var hasTask = !string.IsNullOrWhiteSpace(taskName) && taskName != "All";

            return status switch
            {
                ScheduledTaskExecutionStatus.Success => occurrence => (!hasTask || occurrence.CronTicker.Function == taskName) && occurrence.Status == TickerStatus.Done,
                ScheduledTaskExecutionStatus.Failed => occurrence => (!hasTask || occurrence.CronTicker.Function == taskName) && occurrence.Status == TickerStatus.Failed,
                ScheduledTaskExecutionStatus.Running => occurrence => (!hasTask || occurrence.CronTicker.Function == taskName) && occurrence.Status != TickerStatus.Done && occurrence.Status != TickerStatus.Failed,
                _ => occurrence => !hasTask || occurrence.CronTicker.Function == taskName
            };
        }

        private static ScheduledTaskAttachmentDto MapAttachmentDto(ScheduledTaskAttachment entity)
        {
            return new ScheduledTaskAttachmentDto
            {
                Id = entity.Id,
                OccurrenceId = entity.OccurrenceId,
                FileName = entity.FileName,
                BucketName = entity.BucketName,
                StoragePath = entity.StoragePath,
                ContentType = entity.ContentType,
                FileSizeBytes = entity.FileSizeBytes,
                CreatedAt = entity.CreatedAt
            };
        }
    }
}
