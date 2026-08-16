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
        public CleanUpOldNotificationsJobTests(ServiceCollectionFixture serviceCollectionFixture) : base(serviceCollectionFixture)
        {
        }

        [Fact]
        public async Task TestCleanUp_ExecutesSuccessfully()
        {
            await ExecuteScopeAsync(async sp =>
            {
                var notificationService = sp.GetRequiredService<INotificationService>();
                var job = new CleanUpOldNotificationsJob(notificationService);

                await job.CleanUp();
            });
        }
    }
}
