using System.Threading;
using System.Threading.Tasks;
using Audex.Application.Enums.Scheduler;

namespace Audex.Application.Interfaces.Scheduler
{
    public interface IScheduleExecutor
    {
        Task ExecuteJobAsync(string taskName, ScheduledTaskTriggerSource triggerSource = ScheduledTaskTriggerSource.Manual, CancellationToken cancellationToken = default);
    }
}
