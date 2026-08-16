using System.Threading.Tasks;
using MoneyManager.Application.Interfaces.Notifications;
using TickerQ.Utilities.Base;

namespace MoneyManager.Application.Jobs
{
    public class CleanUpOldNotificationsJob
    {
        private readonly INotificationService _notificationService;

        public CleanUpOldNotificationsJob(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        // Irrelative notifications removal every day at 00:00
        [TickerFunction(functionName: nameof(CleanUp), cronExpression: "0 0 * * *")]
        public async Task CleanUp()
        {
            // Remove notifications older than 3 months (90 days)
            await _notificationService.CleanUpOldNotifications(olderThanDays: 90);
        }
    }
}
