using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
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
                var authService = sp.GetRequiredService<IAuthService>();
                var notificationService = sp.GetRequiredService<INotificationService>();
                var job = new CleanUpExpiredRefreshTokensJob(authService, notificationService);

                await job.CleanUpExpiredRefreshTokensAsync();
            });
        }
    }
}
