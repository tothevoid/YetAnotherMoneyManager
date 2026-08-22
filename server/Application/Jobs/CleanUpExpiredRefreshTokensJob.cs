using System.Threading.Tasks;
using MoneyManager.Application.Interfaces.Auth;
using MoneyManager.Application.Interfaces.DatabaseBackup;
using MoneyManager.Application.Interfaces.Notifications;
using MoneyManager.Infrastructure.Constants;
using MoneyManager.Infrastructure.Entities.Notifications;
using TickerQ.Utilities.Base;

namespace MoneyManager.Application.Jobs
{
    public class CleanUpExpiredRefreshTokensJob
    {
        private readonly IAuthService _authService;
        private readonly INotificationService _notificationService;
        private readonly IDatabaseStateService _databaseStateService;

        public CleanUpExpiredRefreshTokensJob(
            IAuthService authService,
            INotificationService notificationService,
            IDatabaseStateService databaseStateService)
        {
            _authService = authService;
            _notificationService = notificationService;
            _databaseStateService = databaseStateService;
        }

        // Expired and revoked refresh tokens cleanup every day at 00:00
        [TickerFunction(functionName: nameof(CleanUpExpiredRefreshTokensAsync), cronExpression: "0 0 * * *")]
        public async Task CleanUpExpiredRefreshTokensAsync()
        {
            if (_databaseStateService.IsRestoring)
            {
                return;
            }

            // Remove refresh tokens expired or revoked older than 30 days
            var deletedCount = await _authService.CleanUpExpiredRefreshTokensAsync(olderThanDays: 30);

            if (deletedCount > 0)
            {
                await _notificationService.CreateAsync(
                    title: "Session cleanup",
                    message: $"Removed expired and revoked tokens: {deletedCount}.",
                    severity: NotificationSeverity.Info,
                    category: "Auth",
                    userProfileId: UserProfileConstants.UserProfileId);
            }
        }
    }
}
