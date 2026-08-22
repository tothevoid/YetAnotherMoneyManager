using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using MoneyManager.Application.Interfaces.Notifications;
using MoneyManager.Application.Jobs;
using MoneyManager.Application.Tests.Fixtures;
using Xunit;

namespace MoneyManager.Application.Tests.Jobs
{
    public class CleanUpOldNotificationsJobTests : TestBase
    {
        public CleanUpOldNotificationsJobTests(ServiceProviderFixture serviceProviderFixture) : base(serviceProviderFixture)
        {
        }

        [Fact]
        public async Task TestCleanUp_ExecutesSuccessfully()
        {
            await ExecuteScopeAsync(async sp =>
            {
                var notificationService = sp.GetRequiredService<INotificationService>();
                var databaseStateService = sp.GetRequiredService<MoneyManager.Application.Interfaces.DatabaseBackup.IDatabaseStateService>();
                var job = new CleanUpOldNotificationsJob(notificationService, databaseStateService);

                await job.CleanUp();
            });
        }
    }
}
