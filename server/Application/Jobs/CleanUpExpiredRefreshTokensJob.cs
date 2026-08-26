using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using MoneyManager.Application.Attributes.Scheduler;
using MoneyManager.Application.DTO.Scheduler;
using MoneyManager.Application.Enums.Scheduler;
using MoneyManager.Application.Interfaces.Auth;
using MoneyManager.Application.Interfaces.DatabaseBackup;
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
        displayName: "Clean Up Session Tokens",
        description: "Remove expired and revoked JWT refresh tokens older than 30 days",
        category: "Auth",
        defaultCronExpression: "0 0 0 * * *")]
    public class CleanUpExpiredRefreshTokensJob : ScheduledJobBase
    {
        private readonly IAuthService _authService;
        private readonly INotificationService _notificationService;

        public CleanUpExpiredRefreshTokensJob(
            IAuthService authService,
            INotificationService notificationService,
            IDatabaseStateService databaseStateService,
            ISchedulerAttachmentService attachmentService,
            IServerNotifier serverNotifier)
            : base(databaseStateService, attachmentService, serverNotifier)
        {
            _authService = authService;
            _notificationService = notificationService;
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
                await _notificationService.CreateAsync(
                    title: "Session Tokens Clean Up",
                    message: $"Removed {deletedCount} expired or revoked tokens.",
                    severity: NotificationSeverity.Info,
                    category: "Auth",
                    userProfileId: UserProfileConstants.UserProfileId);
            }

            return JobExecutionResult.Success($"Successfully removed {deletedCount} expired session tokens");
        }
    }
}

