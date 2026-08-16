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

        public async Task<IEnumerable<NotificationDto>> GetAll(int pageIndex = 1, int recordsQuantity = 15, bool onlyUnread = false, string category = null)
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

            var notifications = await _notificationRepo.GetAll(builder.GetQuery());
            return _mapper.Map(notifications);
        }

        public async Task<PaginationConfigDto> GetPagination(bool onlyUnread = false, string category = null)
        {
            var recordsQuantity = await _notificationRepo.GetCount(GetNotificationFilter(onlyUnread, category));
            return new PaginationConfigDto
            {
                PageSize = 15,
                RecordsQuantity = recordsQuantity
            };
        }

        public async Task<int> GetUnreadCount()
        {
            var unread = await _notificationRepo.GetAll(
                filter: n => n.UserProfileId == UserProfileConstants.UserProfileId && !n.IsRead,
                disableTracking: true);

            return unread.Count();
        }

        public async Task CleanUpOldNotifications(int olderThanDays = 90)
        {
            var threshold = DateTime.UtcNow.AddDays(-olderThanDays);
            var oldReadNotifications = (await _notificationRepo.GetAll(
                filter: n => n.UserProfileId == UserProfileConstants.UserProfileId && n.IsRead && n.CreatedAt < threshold,
                disableTracking: false)).ToList();

            if (oldReadNotifications.Count == 0) return;

            foreach (var item in oldReadNotifications)
            {
                await _notificationRepo.Delete(item.Id);
            }

            await _db.Commit();
        }

        private static readonly JsonSerializerOptions JsonOptions = new() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

        private static string BuildNotificationReceivedMessage(NotificationDto dto) =>
            $"{{\"type\":\"NotificationReceived\",\"payload\":{JsonSerializer.Serialize(dto, JsonOptions)}}}";

        private static string BuildNotificationReadMessage(Guid notificationId) =>
            $"{{\"type\":\"NotificationRead\",\"notificationId\":\"{notificationId}\"}}";

        private static string BuildAllNotificationsReadMessage() =>
            "{\"type\":\"AllNotificationsRead\"}";

        public async Task<NotificationDto> Create(
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

            await _notificationRepo.Add(entity);
            await _db.Commit();

            var dto = _mapper.Map(entity);
            await _serverNotifier.SendToAll(BuildNotificationReceivedMessage(dto));
            return dto;
        }

        public async Task MarkAsRead(Guid notificationId)
        {
            var item = await _notificationRepo.GetById(notificationId, disableTracking: false);
            if (item != null && !item.IsRead)
            {
                item.IsRead = true;
                item.ReadAt = DateTime.UtcNow;
                _notificationRepo.Update(item);
                await _db.Commit();

                await _serverNotifier.SendToAll(BuildNotificationReadMessage(notificationId));
            }
        }

        public async Task MarkAllAsRead()
        {
            var unreadItems = (await _notificationRepo.GetAll(
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

            await _db.Commit();
            await _serverNotifier.SendToAll(BuildAllNotificationsReadMessage());
        }

        public async Task Delete(Guid notificationId)
        {
            await _notificationRepo.Delete(notificationId);
            await _db.Commit();
        }
    }
}
