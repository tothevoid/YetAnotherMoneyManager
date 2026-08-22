using System.Threading.Tasks;
using MoneyManager.Application.Interfaces.DatabaseBackup;
using MoneyManager.Application.Interfaces.Notifications;
using TickerQ.Utilities.Base;

namespace MoneyManager.Application.Jobs
{
    public class CleanUpOldNotificationsJob
    {
        private readonly INotificationService _notificationService;
        private readonly IDatabaseStateService _databaseStateService;

        public CleanUpOldNotificationsJob(
            INotificationService notificationService,
            IDatabaseStateService databaseStateService)
        {
            _notificationService = notificationService;
            _databaseStateService = databaseStateService;
        }

        // Irrelative notifications removal every day at 00:00
        [TickerFunction(functionName: nameof(CleanUp), cronExpression: "0 0 * * *")]
        public async Task CleanUp()
        {
            if (_databaseStateService.IsRestoring)
            {
                return;
            }

            // Remove notifications older than 3 months (90 days)
            await _notificationService.CleanUpOldNotificationsAsync(olderThanDays: 90);
        }
    }
}
