using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MoneyManager.Application.DTO.Common;
using MoneyManager.Application.DTO.Scheduler;
using MoneyManager.Application.Enums.Scheduler;
using MoneyManager.Infrastructure.Entities.Scheduler;

namespace MoneyManager.Application.Interfaces.Scheduler
{
    public interface ISchedulerJournalService
    {
        Task<IEnumerable<ScheduledTaskJournalDto>> GetJournalAsync(
            int pageIndex = 1,
            int recordsQuantity = 20,
            string taskName = null,
            ScheduledTaskExecutionStatus? status = null,
            ScheduledTaskTriggerSource? triggerSource = null);

        Task<PaginationConfigDto> GetJournalPaginationAsync(
            string taskName = null,
            ScheduledTaskExecutionStatus? status = null,
            ScheduledTaskTriggerSource? triggerSource = null);

        Task<ScheduledTaskAttachment> RecordExecutionAsync(
            string taskName,
            ScheduledTaskExecutionStatus status,
            long durationMs,
            ScheduledTaskTriggerSource triggerSource = ScheduledTaskTriggerSource.Scheduled,
            string logMessage = null,
            string errorMessage = null,
            ScheduledTaskAttachment attachment = null);

        Task NotifyTaskStartedAsync(string taskName);
    }
}
