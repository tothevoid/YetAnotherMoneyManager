using System.Threading;
using System.Threading.Tasks;
using MoneyManager.Application.Enums.Scheduler;

namespace MoneyManager.Application.Interfaces.Scheduler
{
    public interface IScheduleExecutor
    {
        Task ExecuteJobAsync(string taskName, ScheduledTaskTriggerSource triggerSource = ScheduledTaskTriggerSource.Manual, CancellationToken cancellationToken = default);
    }
}
