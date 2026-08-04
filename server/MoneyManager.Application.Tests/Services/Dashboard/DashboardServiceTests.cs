using Microsoft.Extensions.DependencyInjection;
using MoneyManager.Application.Interfaces.Currencies;
using MoneyManager.Application.Interfaces.User;
using MoneyManager.Application.Tests.Fixtures;

namespace MoneyManager.Application.Tests.Services.Dashboard
{
    public class DashboardServiceTests : TestBase
    {
        public DashboardServiceTests(ServiceCollectionFixture serviceCollectionFixture) : base(serviceCollectionFixture)
        {
        }

        [Fact]
        public async Task TestGetDashboard_ReturnsGlobalDashboardDto()
        {
            var dashboard = await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<IDashboardService>();
                return await service.GetDashboard();
            });

            //TODO: add mock data
            Assert.NotNull(dashboard);
            Assert.NotNull(dashboard.AccountsGlobalDashboard);
            Assert.NotNull(dashboard.BrokerAccountsGlobalDashboard);
            Assert.NotNull(dashboard.DebtsGlobalDashboard);
            Assert.NotNull(dashboard.DepositsGlobalDashboard);
            Assert.NotNull(dashboard.TransactionsGlobalDashboard);
            Assert.NotNull(dashboard.CryptoAccountsGlobalDashboard);
            Assert.NotNull(dashboard.BanksGlobalDashboard);
        }
    }
}
