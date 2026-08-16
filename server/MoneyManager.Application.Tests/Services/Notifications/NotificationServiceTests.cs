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
        public NotificationServiceTests(ServiceCollectionFixture serviceCollectionFixture) : base(serviceCollectionFixture)
        {
        }

        [Fact]
        public async Task TestCreateAndGetAll_ReturnsCreatedNotification()
        {
            var created = await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<INotificationService>();
                return await service.Create(
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
                return (await service.GetAll()).ToList();
            });

            Assert.Contains(all, n => n.Id == created.Id);
        }

        [Fact]
        public async Task TestGetUnreadCount_And_MarkAsRead()
        {
            var notification = await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<INotificationService>();
                return await service.Create("Unread Notification", "Message", NotificationSeverity.Warning);
            });

            var unreadCountBefore = await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<INotificationService>();
                return await service.GetUnreadCount();
            });

            Assert.True(unreadCountBefore > 0);

            await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<INotificationService>();
                await service.MarkAsRead(notification.Id);
            });

            var all = await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<INotificationService>();
                return (await service.GetAll()).ToList();
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
                await service.Create("Notification 1", "Msg 1", NotificationSeverity.Info);
                await service.Create("Notification 2", "Msg 2", NotificationSeverity.Danger);
            });

            await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<INotificationService>();
                await service.MarkAllAsRead();
            });

            var unreadCount = await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<INotificationService>();
                return await service.GetUnreadCount();
            });

            Assert.Equal(0, unreadCount);
        }

        [Fact]
        public async Task TestDelete()
        {
            var notification = await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<INotificationService>();
                return await service.Create("To Delete", "Delete Msg", NotificationSeverity.Success);
            });

            await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<INotificationService>();
                await service.Delete(notification.Id);
            });

            var all = await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<INotificationService>();
                return (await service.GetAll()).ToList();
            });

            Assert.DoesNotContain(all, n => n.Id == notification.Id);
        }

        [Fact]
        public async Task TestCleanUpOldNotifications_RemovesOnlyOldReadNotifications()
        {
            var notification = await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<INotificationService>();
                var created = await service.Create("Old Notification", "To be cleaned", NotificationSeverity.Info);
                await service.MarkAsRead(created.Id);
                return created;
            });

            await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<INotificationService>();
                // Clean with olderThanDays: -1 (all read notifications before tomorrow)
                await service.CleanUpOldNotifications(olderThanDays: -1);
            });

            var all = await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<INotificationService>();
                return (await service.GetAll(recordsQuantity: 100)).ToList();
            });

            Assert.DoesNotContain(all, n => n.Id == notification.Id);
        }

        [Fact]
        public async Task TestGetPagination_ReturnsCorrectConfig()
        {
            var notification = await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<INotificationService>();
                return await service.Create("Paginated Notification", "Msg", NotificationSeverity.Info, category: "TestCategory");
            });

            var pagination = await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<INotificationService>();
                return await service.GetPagination(category: "TestCategory");
            });

            Assert.NotNull(pagination);
            Assert.Equal(15, pagination.PageSize);
            Assert.True(pagination.RecordsQuantity >= 1);
        }
    }
}
