using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MoneyManager.Application.DTO.Scheduler;
using MoneyManager.Application.Enums.Scheduler;

namespace MoneyManager.Application.Interfaces.Scheduler
{
    public interface IScheduledJobRegistry
    {
        IReadOnlyList<ScheduledJobDescriptor> GetAllDescriptors();

        ScheduledJobDescriptor GetDescriptor(string taskName);

        bool TryGetDescriptor(string taskName, out ScheduledJobDescriptor descriptor);

        Task ExecuteJobAsync(string taskName, ScheduledTaskTriggerSource triggerSource = ScheduledTaskTriggerSource.Manual, CancellationToken cancellationToken = default);
    }
}
