using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MoneyManager.Application.Enums.Scheduler;
using MoneyManager.Application.Interfaces.Scheduler;
using MoneyManager.Infrastructure.Entities.Scheduler;
using MoneyManager.Infrastructure.Interfaces.Database;

namespace MoneyManager.Application.Services.Scheduler
{
    public class ScheduleExecutor : IScheduleExecutor
    {
        private readonly IUnitOfWork _db;
        private readonly IRepository<ScheduledCronTicker> _tickerRepo;
        private readonly IScheduledJobRegistry _jobRegistry;
        private readonly IEnumerable<IScheduledJob> _jobs;
        private readonly ILogger<ScheduleExecutor> _logger;

        public ScheduleExecutor(
            IUnitOfWork unitOfWork,
            IScheduledJobRegistry jobRegistry,
            IEnumerable<IScheduledJob> jobs,
            ILogger<ScheduleExecutor> logger)
        {
            _db = unitOfWork;
            _tickerRepo = unitOfWork.CreateRepository<ScheduledCronTicker>();
            _jobRegistry = jobRegistry;
            _jobs = jobs;
            _logger = logger;
        }

        public async Task ExecuteJobAsync(
            string taskName,
            ScheduledTaskTriggerSource triggerSource = ScheduledTaskTriggerSource.Manual,
            CancellationToken cancellationToken = default)
        {
            if (!_jobRegistry.TryGetDescriptor(taskName, out var descriptor))
            {
                throw new ArgumentException($"Unknown task '{taskName}'", nameof(taskName));
            }

            var ticker = await _tickerRepo.FindAsync(t => t.Function == taskName);
            if (ticker == null)
            {
                throw new InvalidOperationException($"Task '{taskName}' is not configured/registered by user. Create a schedule first.");
            }

            var job = _jobs.FirstOrDefault(j => j.GetType() == descriptor.JobType);
            if (job == null)
            {
                throw new InvalidOperationException($"Job instance for '{descriptor.JobType.Name}' was not found in DI container.");
            }

            await job.ExecuteAsync(triggerSource, cancellationToken);
        }
    }
}
