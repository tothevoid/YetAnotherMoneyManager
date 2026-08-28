using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using MoneyManager.Application.Attributes.Scheduler;
using MoneyManager.Application.Constants;
using MoneyManager.Application.DTO.Scheduler;
using MoneyManager.Application.Enums.Scheduler;
using MoneyManager.Application.Interfaces.DatabaseBackup;
using MoneyManager.Application.Interfaces.Localization;
using MoneyManager.Application.Interfaces.Notifications;
using MoneyManager.Application.Interfaces.Scheduler;
using MoneyManager.Infrastructure.Constants;
using MoneyManager.Infrastructure.Interfaces.Messages;
using TickerQ.Utilities.Base;

namespace MoneyManager.Application.Jobs
{
    [ScheduledJob(
        taskName: "CleanUpOldNotifications",
        displayNameKey: LocalizationKeys.Jobs.CleanUpOldNotifications.Name,
        descriptionKey: LocalizationKeys.Jobs.CleanUpOldNotifications.Description,
        categoryKey: LocalizationKeys.Jobs.Categories.System,
        defaultCronExpression: "0 0 0 * * *")]
    public class CleanUpOldNotificationsJob : ScheduledJobBase
    {
        private readonly INotificationService _notificationService;
        private readonly ILocalizationService _localizer;

        public CleanUpOldNotificationsJob(
            INotificationService notificationService,
            ILocalizationService localizer,
            IDatabaseStateService databaseStateService,
            ISchedulerAttachmentService attachmentService,
            IServerNotifier serverNotifier)
            : base(databaseStateService, attachmentService, serverNotifier)
        {
            _notificationService = notificationService;
            _localizer = localizer;
        }

        [TickerFunction(functionName: "CleanUpOldNotifications")]
        public async Task CleanUp(
            TickerFunctionContext context,
            CancellationToken cancellationToken = default)
        {
            await ExecuteAsync(
                triggerSource: ScheduledTaskTriggerSource.Scheduled,
                cancellationToken: cancellationToken,
                occurrenceId: context.Id);
        }

        protected override async Task<JobExecutionResult> ExecuteCoreAsync(
            ScheduledTaskTriggerSource triggerSource,
            CancellationToken cancellationToken)
        {
            await _notificationService.CleanUpOldNotificationsAsync(olderThanDays: 90);
            var logMessage = await _localizer.GetForUserAsync(LocalizationKeys.Scheduler.CleanUpNotificationsSuccess, UserProfileConstants.UserProfileId, 90);
            return JobExecutionResult.Success(logMessage);
        }
    }
}

