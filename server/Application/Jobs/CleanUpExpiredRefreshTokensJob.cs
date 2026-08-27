using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using MoneyManager.Application.Attributes.Scheduler;
using MoneyManager.Application.Constants;
using MoneyManager.Application.DTO.Scheduler;
using MoneyManager.Application.Enums.Scheduler;
using MoneyManager.Application.Interfaces.Auth;
using MoneyManager.Application.Interfaces.DatabaseBackup;
using MoneyManager.Application.Interfaces.Localization;
using MoneyManager.Application.Interfaces.Notifications;
using MoneyManager.Application.Interfaces.Scheduler;
using MoneyManager.Infrastructure.Constants;
using MoneyManager.Infrastructure.Entities.Notifications;
using MoneyManager.Infrastructure.Interfaces.Messages;
using TickerQ.Utilities.Base;

namespace MoneyManager.Application.Jobs
{
    [ScheduledJob(
        taskName: "CleanUpExpiredRefreshTokens",
        displayNameKey: LocalizationKeys.Jobs.CleanUpExpiredTokens.Name,
        descriptionKey: LocalizationKeys.Jobs.CleanUpExpiredTokens.Description,
        categoryKey: LocalizationKeys.Jobs.Categories.Auth,
        defaultCronExpression: "0 0 0 * * *")]
    public class CleanUpExpiredRefreshTokensJob : ScheduledJobBase
    {
        private readonly IAuthService _authService;
        private readonly INotificationService _notificationService;
        private readonly ILocalizationService _localizer;

        public CleanUpExpiredRefreshTokensJob(
            IAuthService authService,
            INotificationService notificationService,
            ILocalizationService localizer,
            IDatabaseStateService databaseStateService,
            ISchedulerAttachmentService attachmentService,
            IServerNotifier serverNotifier)
            : base(databaseStateService, attachmentService, serverNotifier)
        {
            _authService = authService;
            _notificationService = notificationService;
            _localizer = localizer;
        }

        [TickerFunction(functionName: "CleanUpExpiredRefreshTokens")]
        public async Task CleanUpExpiredRefreshTokensAsync(
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
            var deletedCount = await _authService.CleanUpExpiredRefreshTokensAsync(olderThanDays: 30);

            if (deletedCount > 0)
            {
                var title = await _localizer.GetForUserAsync(LocalizationKeys.Notifications.SessionCleanupTitle, UserProfileConstants.UserProfileId);
                var message = await _localizer.GetForUserAsync(LocalizationKeys.Notifications.SessionCleanupMessage, UserProfileConstants.UserProfileId, deletedCount);

                await _notificationService.CreateAsync(
                    title: title,
                    message: message,
                    severity: NotificationSeverity.Info,
                    category: "Auth",
                    userProfileId: UserProfileConstants.UserProfileId);
            }

            var logMessage = await _localizer.GetForUserAsync(LocalizationKeys.Scheduler.CleanUpSessionsSuccess, UserProfileConstants.UserProfileId, deletedCount);
            return JobExecutionResult.Success(logMessage);
        }
    }
}

