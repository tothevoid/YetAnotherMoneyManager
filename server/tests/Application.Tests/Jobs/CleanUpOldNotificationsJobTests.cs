using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using MoneyManager.Application.Enums.Scheduler;
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
                var job = sp.GetRequiredService<CleanUpOldNotificationsJob>();

                await job.ExecuteAsync(triggerSource: ScheduledTaskTriggerSource.Manual);
            });
        }
    }
}
