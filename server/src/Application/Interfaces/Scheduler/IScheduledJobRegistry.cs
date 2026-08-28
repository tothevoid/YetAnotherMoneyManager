using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Audex.Application.DTO.Scheduler;
using Audex.Application.Enums.Scheduler;

namespace Audex.Application.Interfaces.Scheduler
{
    public interface IScheduledJobRegistry
    {
        IReadOnlyList<ScheduledJobDescriptor> GetAllDescriptors();

        ScheduledJobDescriptor GetDescriptor(string taskName);

        bool TryGetDescriptor(string taskName, out ScheduledJobDescriptor descriptor);
    }
}
