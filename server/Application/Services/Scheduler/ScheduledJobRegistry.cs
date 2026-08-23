using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using MoneyManager.Application.Attributes.Scheduler;
using MoneyManager.Application.DTO.Scheduler;
using MoneyManager.Application.Enums.Scheduler;
using MoneyManager.Application.Interfaces.Scheduler;

namespace MoneyManager.Application.Services.Scheduler
{
    public class ScheduledJobRegistry : IScheduledJobRegistry
    {
        private readonly Dictionary<string, ScheduledJobDescriptor> _descriptors;
        private readonly IServiceScopeFactory _scopeFactory;

        public ScheduledJobRegistry(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
            _descriptors = ScanScheduledJobs();
        }

        public async Task ExecuteJobAsync(string taskName, ScheduledTaskTriggerSource triggerSource = ScheduledTaskTriggerSource.Manual, CancellationToken cancellationToken = default)
        {
            if (!TryGetDescriptor(taskName, out var descriptor))
            {
                throw new ArgumentException($"Unknown task '{taskName}'", nameof(taskName));
            }

            using var scope = _scopeFactory.CreateScope();
            var job = (IScheduledJob)scope.ServiceProvider.GetRequiredService(descriptor.JobType);
            await job.ExecuteAsync(triggerSource, cancellationToken);
        }

        public IReadOnlyList<ScheduledJobDescriptor> GetAllDescriptors()
        {
            return _descriptors.Values.ToList();
        }

        public ScheduledJobDescriptor GetDescriptor(string taskName)
        {
            if (TryGetDescriptor(taskName, out var descriptor))
            {
                return descriptor;
            }

            throw new KeyNotFoundException($"Scheduled job with task name '{taskName}' was not found in registry.");
        }

        public bool TryGetDescriptor(string taskName, out ScheduledJobDescriptor descriptor)
        {
            if (string.IsNullOrWhiteSpace(taskName))
            {
                descriptor = null;
                return false;
            }

            return _descriptors.TryGetValue(taskName, out descriptor);
        }

        private static Dictionary<string, ScheduledJobDescriptor> ScanScheduledJobs()
        {
            var jobInterface = typeof(IScheduledJob);
            var assembly = jobInterface.Assembly;

            var jobTypes = assembly.GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract && jobInterface.IsAssignableFrom(t));

            var result = new Dictionary<string, ScheduledJobDescriptor>(StringComparer.OrdinalIgnoreCase);

            foreach (var type in jobTypes)
            {
                var attr = type.GetCustomAttribute<ScheduledJobAttribute>();
                if (attr != null)
                {
                    var descriptor = new ScheduledJobDescriptor(
                        JobType: type,
                        TaskName: attr.TaskName,
                        DisplayName: attr.DisplayName,
                        Description: attr.Description,
                        Category: attr.Category,
                        DefaultCronExpression: attr.DefaultCronExpression
                    );

                    result[attr.TaskName] = descriptor;
                }
            }

            return result;
        }
    }
}
