using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using MoneyManager.Application.Interfaces.Notifications;
using MoneyManager.Application.Tests.Fixtures;
using MoneyManager.Infrastructure.Entities.Notifications;
using Xunit;

namespace MoneyManager.Application.Tests.Services.Notifications
{
    public class NotificationServiceTests : TestBase
    {
        public NotificationServiceTests(ServiceProviderFixture serviceProviderFixture) : base(serviceProviderFixture)
        {
        }

        [Fact]
        public async Task TestCreateAndGetAll_ReturnsCreatedNotification()
        {
            var created = await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<INotificationService>();
                return await service.CreateAsync(
                    title: "Test Notification",
                    message: "This is a test notification message",
                    severity: NotificationSeverity.Info,
                    actionUrl: "/broker_accounts",
                    category: "Broker");
            });

            Assert.NotNull(created);
            Assert.NotEqual(Guid.Empty, created.Id);
            Assert.Equal("Test Notification", created.Title);
            Assert.Equal("This is a test notification message", created.Message);
            Assert.Equal(NotificationSeverity.Info, created.Severity);
            Assert.Equal("/broker_accounts", created.ActionUrl);
            Assert.Equal("Broker", created.Category);
            Assert.False(created.IsRead);
            Assert.Null(created.ReadAt);

            var all = await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<INotificationService>();
                return (await service.GetAllAsync()).ToList();
            });

            Assert.Contains(all, n => n.Id == created.Id);
        }

        [Fact]
        public async Task TestGetUnreadCount_ReturnsPositiveCountForUnreadNotifications()
        {
            await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<INotificationService>();
                return await service.CreateAsync("Unread Notification", "Message", NotificationSeverity.Warning);
            });

            var unreadCount = await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<INotificationService>();
                return await service.GetUnreadCountAsync();
            });

            Assert.True(unreadCount > 0);
        }

        [Fact]
        public async Task TestMarkAsRead_UpdatesIsReadAndReadAt()
        {
            var notification = await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<INotificationService>();
                return await service.CreateAsync("Unread Notification", "Message", NotificationSeverity.Warning);
            });

            await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<INotificationService>();
                await service.MarkAsReadAsync(notification.Id);
            });

            var all = await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<INotificationService>();
                return (await service.GetAllAsync()).ToList();
            });

            var updated = all.FirstOrDefault(n => n.Id == notification.Id);
            Assert.NotNull(updated);
            Assert.True(updated.IsRead);
            Assert.NotNull(updated.ReadAt);
        }

        [Fact]
        public async Task TestMarkAllAsRead()
        {
            await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<INotificationService>();
                await service.CreateAsync("Notification 1", "Msg 1", NotificationSeverity.Info);
                await service.CreateAsync("Notification 2", "Msg 2", NotificationSeverity.Danger);
            });

            await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<INotificationService>();
                await service.MarkAllAsReadAsync();
            });

            var unreadCount = await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<INotificationService>();
                return await service.GetUnreadCountAsync();
            });

            Assert.Equal(0, unreadCount);
        }

        [Fact]
        public async Task TestDelete()
        {
            var notification = await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<INotificationService>();
                return await service.CreateAsync("To Delete", "Delete Msg", NotificationSeverity.Success);
            });

            await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<INotificationService>();
                await service.DeleteAsync(notification.Id);
            });

            var all = await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<INotificationService>();
                return (await service.GetAllAsync()).ToList();
            });

            Assert.DoesNotContain(all, n => n.Id == notification.Id);
        }

        [Fact]
        public async Task TestCleanUpOldNotifications_RemovesOnlyOldReadNotifications()
        {
            var notification = await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<INotificationService>();
                var created = await service.CreateAsync("Old Notification", "To be cleaned", NotificationSeverity.Info);
                await service.MarkAsReadAsync(created.Id);
                return created;
            });

            await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<INotificationService>();
                // Clean with olderThanDays: -1 (all read notifications before tomorrow)
                await service.CleanUpOldNotificationsAsync(olderThanDays: -1);
            });

            var all = await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<INotificationService>();
                return (await service.GetAllAsync(recordsQuantity: 100)).ToList();
            });

            Assert.DoesNotContain(all, n => n.Id == notification.Id);
        }

        [Fact]
        public async Task TestGetPagination_ReturnsCorrectConfig()
        {
            var notification = await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<INotificationService>();
                return await service.CreateAsync("Paginated Notification", "Msg", NotificationSeverity.Info, category: "TestCategory");
            });

            var pagination = await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<INotificationService>();
                return await service.GetPaginationAsync(category: "TestCategory");
            });

            Assert.NotNull(pagination);
            Assert.Equal(15, pagination.PageSize);
            Assert.True(pagination.RecordsQuantity >= 1);
        }
    }
}
