using System.Collections.Generic;
using MoneyManager.Application.DTO.Scheduler;

namespace MoneyManager.Application.Interfaces.Scheduler
{
    public interface IScheduledJobRegistry
    {
        IReadOnlyList<ScheduledJobDescriptor> GetAllDescriptors();

        ScheduledJobDescriptor GetDescriptor(string taskName);

        bool TryGetDescriptor(string taskName, out ScheduledJobDescriptor descriptor);
    }
}
