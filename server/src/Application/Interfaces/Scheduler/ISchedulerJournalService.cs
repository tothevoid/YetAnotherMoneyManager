using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Audex.Application.DTO.Common;
using Audex.Application.DTO.Scheduler;
using Audex.Application.Enums.Scheduler;
using Audex.Infrastructure.Entities.Scheduler;

namespace Audex.Application.Interfaces.Scheduler
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
    }
}
