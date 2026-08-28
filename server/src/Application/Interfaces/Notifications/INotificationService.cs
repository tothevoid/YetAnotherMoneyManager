using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MoneyManager.Application.DTO.Common;
using MoneyManager.Application.DTO.Notifications;
using MoneyManager.Infrastructure.Entities.Notifications;

namespace MoneyManager.Application.Interfaces.Notifications
{
    public interface INotificationService
    {
        Task<IEnumerable<NotificationDto>> GetAllAsync(int pageIndex = 1, int recordsQuantity = 15, bool onlyUnread = false, string category = null);

        Task<PaginationConfigDto> GetPaginationAsync(bool onlyUnread = false, string category = null);

        Task<int> GetUnreadCountAsync();

        Task<NotificationDto> CreateAsync(
            string title,
            string message,
            NotificationSeverity severity = NotificationSeverity.Info,
            string actionUrl = null,
            string category = "System",
            Guid? userProfileId = null);

        Task MarkAsReadAsync(Guid notificationId);

        Task MarkAllAsReadAsync();

        Task DeleteAsync(Guid notificationId);

        Task CleanUpOldNotificationsAsync(int olderThanDays = 90);
    }
}
