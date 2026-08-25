using System;
using System.Diagnostics;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using MoneyManager.Application.Attributes.Scheduler;
using MoneyManager.Application.DTO.Scheduler;
using MoneyManager.Application.Enums.Scheduler;
using MoneyManager.Application.Interfaces.DatabaseBackup;
using MoneyManager.Application.Interfaces.Scheduler;

namespace MoneyManager.Application.Jobs
{
    public abstract class ScheduledJobBase : IScheduledJob
    {
        protected readonly IDatabaseStateService _databaseStateService;
        protected readonly ISchedulerJournalService _journalService;

        protected virtual string TaskName =>
            GetType().GetCustomAttribute<ScheduledJobAttribute>()?.TaskName ?? GetType().Name;

        protected ScheduledJobBase(
            IDatabaseStateService databaseStateService,
            ISchedulerJournalService journalService)
        {
            _databaseStateService = databaseStateService;
            _journalService = journalService;
        }

        public async Task ExecuteAsync(
            ScheduledTaskTriggerSource triggerSource = ScheduledTaskTriggerSource.Manual,
            CancellationToken cancellationToken = default)
        {
            if (_databaseStateService.IsRestoring)
            {
                return;
            }

            var stopwatch = Stopwatch.StartNew();
            try
            {
                var result = await ExecuteCoreAsync(triggerSource, cancellationToken);
                stopwatch.Stop();

                await _journalService.RecordExecutionAsync(
                    taskName: TaskName,
                    status: ScheduledTaskExecutionStatus.Done,
                    durationMs: stopwatch.ElapsedMilliseconds,
                    triggerSource: triggerSource,
                    logMessage: result?.LogMessage,
                    errorMessage: null,
                    attachment: result?.Attachment);

                await OnSuccessAsync(triggerSource, result, cancellationToken);
            }
            catch (Exception ex)
            {
                stopwatch.Stop();

                await _journalService.RecordExecutionAsync(
                    taskName: TaskName,
                    status: ScheduledTaskExecutionStatus.Failed,
                    durationMs: stopwatch.ElapsedMilliseconds,
                    triggerSource: triggerSource,
                    logMessage: null,
                    errorMessage: ex.Message,
                    attachment: null);

                await OnFailureAsync(triggerSource, ex, cancellationToken);

                throw;
            }
        }

        protected abstract Task<JobExecutionResult> ExecuteCoreAsync(
            ScheduledTaskTriggerSource triggerSource,
            CancellationToken cancellationToken);

        protected virtual Task OnSuccessAsync(
            ScheduledTaskTriggerSource triggerSource,
            JobExecutionResult result,
            CancellationToken cancellationToken) => Task.CompletedTask;

        protected virtual Task OnFailureAsync(
            ScheduledTaskTriggerSource triggerSource,
            Exception exception,
            CancellationToken cancellationToken) => Task.CompletedTask;
    }
}
