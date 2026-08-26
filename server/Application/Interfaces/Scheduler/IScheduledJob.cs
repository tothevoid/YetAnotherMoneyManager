using System;
using System.Threading;
using System.Threading.Tasks;
using MoneyManager.Application.Enums.Scheduler;

namespace MoneyManager.Application.Interfaces.Scheduler
{
    public interface IScheduledJob
    {
        Task ExecuteAsync(
            ScheduledTaskTriggerSource triggerSource = ScheduledTaskTriggerSource.Scheduled,
            CancellationToken cancellationToken = default,
            Guid? occurrenceId = null);
    }
}
