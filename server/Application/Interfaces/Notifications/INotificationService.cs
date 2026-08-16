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
        Task<IEnumerable<NotificationDto>> GetAll(int pageIndex = 1, int recordsQuantity = 15, bool onlyUnread = false, string category = null);

        Task<PaginationConfigDto> GetPagination(bool onlyUnread = false, string category = null);

        Task<int> GetUnreadCount();

        Task<NotificationDto> Create(
            string title,
            string message,
            NotificationSeverity severity = NotificationSeverity.Info,
            string actionUrl = null,
            string category = "System",
            Guid? userProfileId = null);

        Task MarkAsRead(Guid notificationId);

        Task MarkAllAsRead();

        Task Delete(Guid notificationId);

        Task CleanUpOldNotifications(int olderThanDays = 90);
    }
}
