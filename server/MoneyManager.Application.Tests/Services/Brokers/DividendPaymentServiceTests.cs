using Microsoft.Extensions.DependencyInjection;
using MoneyManager.Application.DTO.Brokers;
using MoneyManager.Application.DTO.Securities;
using MoneyManager.Application.Interfaces.Brokers;
using MoneyManager.Application.Interfaces.Securities;
using MoneyManager.Application.Tests.Fixtures;
using MoneyManager.Infrastructure.Constants;

namespace MoneyManager.Application.Tests.Services.Brokers
{
    public class DividendPaymentServiceTests : TestBase
    {
        public DividendPaymentServiceTests(ServiceCollectionFixture serviceCollectionFixture) : base(serviceCollectionFixture)
        {
        }

        [Fact]
        public async Task TestAddAndGetEarnings()
        {
            var (brokerAccountId, dividendId) = await SetupDependencies();

            var paymentId = await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<IDividendPaymentService>();
                return await service.Add(new DividendPaymentDto
                {
                    BrokerAccountId = brokerAccountId,
                    DividendId = dividendId,
                    SecuritiesQuantity = 100,
                    Tax = 5m,
                    ReceivedAt = DateOnly.FromDateTime(DateTime.Now)
                });
            });

            Assert.NotEqual(Guid.Empty, paymentId);

            var totalEarnings = await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<IDividendPaymentService>();
                return await service.GetEarnings();
            });

            Assert.True(totalEarnings > 0);

            var accountEarnings = await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<IDividendPaymentService>();
                return await service.GetEarningsByBrokerAccount(brokerAccountId);
            });

            Assert.True(accountEarnings > 0);
        }

        [Fact]
        public async Task TestUpdateAndDelete()
        {
            var (brokerAccountId, dividendId) = await SetupDependencies();

            var paymentId = await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<IDividendPaymentService>();
                return await service.Add(new DividendPaymentDto
                {
                    BrokerAccountId = brokerAccountId,
                    DividendId = dividendId,
                    SecuritiesQuantity = 50,
                    Tax = 2m,
                    ReceivedAt = DateOnly.FromDateTime(DateTime.Now)
                });
            });

            await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<IDividendPaymentService>();
                await service.Update(new DividendPaymentDto
                {
                    Id = paymentId,
                    BrokerAccountId = brokerAccountId,
                    DividendId = dividendId,
                    SecuritiesQuantity = 200,
                    Tax = 10m,
                    ReceivedAt = DateOnly.FromDateTime(DateTime.Now)
                });
            });

            await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<IDividendPaymentService>();
                await service.Delete(paymentId);
            });
        }

        private async Task<(Guid brokerAccountId, Guid dividendId)> SetupDependencies()
        {
            var brokerId = await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<IBrokerService>();
                return await service.Add(new BrokerDTO { Name = "Div Payment Broker" });
            });

            var typeId = await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<IBrokerAccountTypeService>();
                return await service.Add(new BrokerAccountTypeDTO { Name = "Div Payment Type" });
            });

            var brokerAccountId = await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<IBrokerAccountService>();
                return await service.Add(new BrokerAccountDTO
                {
                    Name = "Div Payment Broker Acc",
                    BrokerId = brokerId,
                    TypeId = typeId,
                    CurrencyId = CurrencyConstants.USD
                });
            });

            var secTypeId = await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<ISecurityTypeService>();
                return await service.Add(new SecurityTypeDTO { Name = "Div Sec Type" });
            });

            var securityId = await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<ISecurityService>();
                var sec = await service.Add(new SecurityDTO
                {
                    Name = "Div Security",
                    Ticker = "DIVP",
                    TypeId = secTypeId,
                    CurrencyId = CurrencyConstants.USD,
                    ActualPrice = 100m
                }, null);
                return sec.Id;
            });

            var today = DateOnly.FromDateTime(DateTime.Now);
            var dividendId = await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<IDividendService>();
                return await service.Add(new DividendDto
                {
                    SecurityId = securityId,
                    Amount = 1.5m,
                    DeclarationDate = today.AddMonths(-1),
                    SnapshotDate = today
                });
            });

            return (brokerAccountId, dividendId);
        }
    }
}
