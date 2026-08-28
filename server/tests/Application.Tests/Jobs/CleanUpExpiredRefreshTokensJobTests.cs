using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using MoneyManager.Application.Enums.Scheduler;
using MoneyManager.Application.Interfaces.Auth;
using MoneyManager.Application.Interfaces.Notifications;
using MoneyManager.Application.Jobs;
using MoneyManager.Application.Tests.Fixtures;
using Xunit;

namespace MoneyManager.Application.Tests.Jobs
{
    [Trait("Category", "Auth")]
    public class CleanUpExpiredRefreshTokensJobTests : TestBase
    {
        public CleanUpExpiredRefreshTokensJobTests(ServiceProviderFixture serviceProviderFixture) : base(serviceProviderFixture)
        {
        }

        [Fact]
        public async Task TestCleanUpExpiredRefreshTokens_ExecutesSuccessfully()
        {
            await ExecuteScopeAsync(async sp =>
            {
                var job = sp.GetRequiredService<CleanUpExpiredRefreshTokensJob>();

                await job.ExecuteAsync(triggerSource: ScheduledTaskTriggerSource.Manual);
            });
        }
    }
}
