using Microsoft.Extensions.DependencyInjection;
using MoneyManager.Application.DTO.Accounts;
using MoneyManager.Application.DTO.Brokers;
using MoneyManager.Application.Interfaces.Accounts;
using MoneyManager.Application.Interfaces.Brokers;
using MoneyManager.Application.Tests.Fixtures;
using MoneyManager.Infrastructure.Constants;

namespace MoneyManager.Application.Tests.Services.Brokers
{
    public class BrokerAccountSummaryServiceTests : TestBase
    {
        public BrokerAccountSummaryServiceTests(ServiceProviderFixture serviceProviderFixture) : base(serviceProviderFixture)
        {
        }

        [Fact]
        public async Task TestGetSummary_And_GetSummaryByBrokerAccount()
        {
            var (brokerAccountId, accountId) = await SetupDependencies();

            // Add a funds transfer
            await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<IBrokerAccountFundsTransferService>();
                await service.AddAsync(new BrokerAccountFundsTransferDto
                {
                    BrokerAccountId = brokerAccountId,
                    AccountId = accountId,
                    Amount = 5000m,
                    Income = true,
                    Date = DateTime.UtcNow
                });
            });

            var summary = await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<IBrokerAccountSummaryService>();
                return await service.GetSummaryAsync();
            });

            Assert.NotNull(summary);
            Assert.NotNull(summary.TransferStats);
            Assert.True(summary.TransferStats.TotalDeposited >= 5000m);

            var accountSummary = await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<IBrokerAccountSummaryService>();
                return await service.GetSummaryByBrokerAccountAsync(brokerAccountId);
            });

            Assert.NotNull(accountSummary);
            Assert.Equal(5000m, accountSummary.TransferStats.TotalDeposited);
        }

        [Fact]
        public async Task TestGetTransfersHistory_MonthAndYear_ReturnsHistory()
        {
            var (brokerAccountId, accountId) = await SetupDependencies();

            // Add a funds transfer
            await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<IBrokerAccountFundsTransferService>();
                await service.AddAsync(new BrokerAccountFundsTransferDto
                {
                    BrokerAccountId = brokerAccountId,
                    AccountId = accountId,
                    Amount = 5000m,
                    Income = true,
                    Date = DateTime.UtcNow
                });
            });

            var now = DateTime.UtcNow;

            var monthHistory = await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<IBrokerAccountSummaryService>();
                return await service.GetMonthTransfersHistoryByBrokerAccountAsync(brokerAccountId, now.Month, now.Year);
            });

            Assert.NotNull(monthHistory);
            Assert.NotEmpty(monthHistory);

            var yearHistory = await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<IBrokerAccountSummaryService>();
                return await service.GetYearTransfersHistoryByBrokerAccountAsync(brokerAccountId, now.Year);
            });

            Assert.NotNull(yearHistory);
            Assert.NotEmpty(yearHistory);
        }

        private async Task<(Guid brokerAccountId, Guid accountId)> SetupDependencies()
        {
            var brokerId = await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<IBrokerService>();
                return await service.AddAsync(new BrokerDto { Name = "Summary Broker" });
            });

            var typeId = await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<IBrokerAccountTypeService>();
                return await service.AddAsync(new BrokerAccountTypeDto { Name = "Summary Acc Type" });
            });

            var brokerAccountId = await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<IBrokerAccountService>();
                return await service.AddAsync(new BrokerAccountDto
                {
                    Name = "Summary Broker Acc",
                    BrokerId = brokerId,
                    TypeId = typeId,
                    CurrencyId = CurrencyConstants.USD,
                    MainCurrencyAmount = 2000m
                });
            });

            var accountId = await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<IAccountService>();
                return await service.AddAsync(new AccountDto
                {
                    Active = true,
                    Name = "Summary Linked Card",
                    AccountTypeId = AccountTypeConstants.Cash,
                    CurrencyId = CurrencyConstants.USD,
                    Balance = 10000m,
                    CreatedOn = DateOnly.FromDateTime(DateTime.UtcNow)
                });
            });

            return (brokerAccountId, accountId);
        }
    }
}
