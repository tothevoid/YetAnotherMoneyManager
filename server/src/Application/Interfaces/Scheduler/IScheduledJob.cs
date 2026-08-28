using System;
using System.Threading;
using System.Threading.Tasks;
using Audex.Application.Enums.Scheduler;

namespace Audex.Application.Interfaces.Scheduler
{
    public interface IScheduledJob
    {
        Task ExecuteAsync(
            ScheduledTaskTriggerSource triggerSource = ScheduledTaskTriggerSource.Scheduled,
            CancellationToken cancellationToken = default,
            Guid? occurrenceId = null);
    }
}
