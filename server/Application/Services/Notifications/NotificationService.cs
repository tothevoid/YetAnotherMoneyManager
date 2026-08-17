using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text.Json;
using System.Threading.Tasks;
using MoneyManager.Application.DTO.Common;
using MoneyManager.Application.DTO.Notifications;
using MoneyManager.Application.Interfaces.Notifications;
using MoneyManager.Application.Mappings;
using MoneyManager.Infrastructure.Constants;
using MoneyManager.Infrastructure.Entities.Notifications;
using MoneyManager.Infrastructure.Interfaces.Database;
using MoneyManager.Infrastructure.Interfaces.Messages;
using MoneyManager.Infrastructure.Queries;

namespace MoneyManager.Application.Services.Notifications
{
    public class NotificationService(
        IUnitOfWork uow,
        ApplicationMapper mapper,
        IServerNotifier serverNotifier) : INotificationService
    {
        private readonly IUnitOfWork _db = uow;
        private readonly IRepository<Notification> _notificationRepo = uow.CreateRepository<Notification>();
        private readonly ApplicationMapper _mapper = mapper;
        private readonly IServerNotifier _serverNotifier = serverNotifier;

        private static Expression<Func<Notification, bool>> GetNotificationFilter(bool onlyUnread, string category)
        {
            var hasCategory = !string.IsNullOrEmpty(category) && category != "All";
            return n => n.UserProfileId == UserProfileConstants.UserProfileId &&
                        (!onlyUnread || !n.IsRead) &&
                        (!hasCategory || n.Category == category);
        }

        public async Task<IEnumerable<NotificationDto>> GetAllAsync(int pageIndex = 1, int recordsQuantity = 15, bool onlyUnread = false, string category = null)
        {
            var builder = new ComplexQueryBuilder<Notification>()
                .AddFilter(GetNotificationFilter(onlyUnread, category))
                .DisableTracking();

            if (pageIndex > 0 && recordsQuantity > 0)
            {
                builder.AddPagination(pageIndex, recordsQuantity, n => n.CreatedAt, isDescending: true);
            }
            else
            {
                builder.AddOrder(n => n.CreatedAt, isDescending: true);
                if (recordsQuantity > 0)
                {
                    var query = builder.GetQuery();
                    query.RecordsLimit = recordsQuantity;
                }
            }

            var notifications = await _notificationRepo.GetAllAsync(builder.GetQuery());
            return _mapper.Map(notifications);
        }

        public async Task<PaginationConfigDto> GetPaginationAsync(bool onlyUnread = false, string category = null)
        {
            var recordsQuantity = await _notificationRepo.GetCountAsync(GetNotificationFilter(onlyUnread, category));
            return new PaginationConfigDto
            {
                PageSize = 15,
                RecordsQuantity = recordsQuantity
            };
        }

        public async Task<int> GetUnreadCountAsync()
        {
            var unread = await _notificationRepo.GetAllAsync(
                filter: n => n.UserProfileId == UserProfileConstants.UserProfileId && !n.IsRead,
                disableTracking: true);

            return unread.Count();
        }

        public async Task CleanUpOldNotificationsAsync(int olderThanDays = 90)
        {
            var threshold = DateTime.UtcNow.AddDays(-olderThanDays);
            var oldReadNotifications = (await _notificationRepo.GetAllAsync(
                filter: n => n.UserProfileId == UserProfileConstants.UserProfileId && n.IsRead && n.CreatedAt < threshold,
                disableTracking: false)).ToList();

            if (oldReadNotifications.Count == 0) return;

            foreach (var item in oldReadNotifications)
            {
                await _notificationRepo.DeleteAsync(item.Id);
            }

            await _db.CommitAsync();
        }

        private static readonly JsonSerializerOptions JsonOptions = new() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

        private static string BuildNotificationReceivedMessage(NotificationDto dto) =>
            $"{{\"type\":\"NotificationReceived\",\"payload\":{JsonSerializer.Serialize(dto, JsonOptions)}}}";

        private static string BuildNotificationReadMessage(Guid notificationId) =>
            $"{{\"type\":\"NotificationRead\",\"notificationId\":\"{notificationId}\"}}";

        private static string BuildAllNotificationsReadMessage() =>
            "{\"type\":\"AllNotificationsRead\"}";

        public async Task<NotificationDto> CreateAsync(
            string title,
            string message,
            NotificationSeverity severity = NotificationSeverity.Info,
            string actionUrl = null,
            string category = "System",
            Guid? userProfileId = null)
        {
            var entity = new Notification
            {
                Id = Guid.NewGuid(),
                UserProfileId = userProfileId ?? UserProfileConstants.UserProfileId,
                Title = title,
                Message = message,
                Severity = severity,
                ActionUrl = actionUrl,
                Category = category,
                IsRead = false,
                CreatedAt = DateTime.UtcNow
            };

            await _notificationRepo.AddAsync(entity);
            await _db.CommitAsync();

            var dto = _mapper.Map(entity);
            await _serverNotifier.SendToAllAsync(BuildNotificationReceivedMessage(dto));
            return dto;
        }

        public async Task MarkAsReadAsync(Guid notificationId)
        {
            var item = await _notificationRepo.GetByIdAsync(notificationId, disableTracking: false);
            if (item != null && !item.IsRead)
            {
                item.IsRead = true;
                item.ReadAt = DateTime.UtcNow;
                _notificationRepo.Update(item);
                await _db.CommitAsync();

                await _serverNotifier.SendToAllAsync(BuildNotificationReadMessage(notificationId));
            }
        }

        public async Task MarkAllAsReadAsync()
        {
            var unreadItems = (await _notificationRepo.GetAllAsync(
                filter: n => n.UserProfileId == UserProfileConstants.UserProfileId && !n.IsRead,
                disableTracking: false)).ToList();

            if (unreadItems.Count == 0) return;

            var now = DateTime.UtcNow;
            foreach (var item in unreadItems)
            {
                item.IsRead = true;
                item.ReadAt = now;
                _notificationRepo.Update(item);
            }

            await _db.CommitAsync();
            await _serverNotifier.SendToAllAsync(BuildAllNotificationsReadMessage());
        }

        public async Task DeleteAsync(Guid notificationId)
        {
            await _notificationRepo.DeleteAsync(notificationId);
            await _db.CommitAsync();
        }
    }
}
