using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using MoneyManager.Application.Attributes.Scheduler;
using MoneyManager.Application.DTO.Scheduler;
using MoneyManager.Application.Enums.Scheduler;
using MoneyManager.Application.Interfaces.DatabaseBackup;
using MoneyManager.Application.Interfaces.Notifications;
using MoneyManager.Application.Interfaces.Scheduler;
using TickerQ.Utilities.Base;

namespace MoneyManager.Application.Jobs
{
    [ScheduledJob(
        taskName: "CleanUpOldNotifications",
        displayName: "Clean Up Old Notifications",
        description: "Remove read system notifications older than 90 days",
        category: "System",
        defaultCronExpression: "0 0 0 * * *")]
    public class CleanUpOldNotificationsJob : ScheduledJobBase
    {
        private readonly INotificationService _notificationService;

        public CleanUpOldNotificationsJob(
            INotificationService notificationService,
            IDatabaseStateService databaseStateService,
            ISchedulerJournalService journalService)
            : base(databaseStateService, journalService)
        {
            _notificationService = notificationService;
        }

        [TickerFunction(functionName: "CleanUpOldNotifications")]
        public async Task CleanUp(
            TickerFunctionContext context = null,
            CancellationToken cancellationToken = default)
        {
            await ExecuteAsync(triggerSource: ScheduledTaskTriggerSource.Scheduled, cancellationToken: cancellationToken);
        }

        protected override async Task<JobExecutionResult> ExecuteCoreAsync(
            ScheduledTaskTriggerSource triggerSource,
            CancellationToken cancellationToken)
        {
            await _notificationService.CleanUpOldNotificationsAsync(olderThanDays: 90);
            return JobExecutionResult.Success("Successfully cleaned up read system notifications older than 90 days");
        }
    }
}

