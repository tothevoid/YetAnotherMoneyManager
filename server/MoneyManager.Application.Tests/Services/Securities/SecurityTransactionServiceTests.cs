using Microsoft.Extensions.DependencyInjection;
using MoneyManager.Application.DTO.Brokers;
using MoneyManager.Application.DTO.Securities;
using MoneyManager.Application.Interfaces.Brokers;
using MoneyManager.Application.Interfaces.Securities;
using MoneyManager.Application.Tests.Fixtures;
using MoneyManager.Infrastructure.Constants;

namespace MoneyManager.Application.Tests.Services.Securities
{
    public class SecurityTransactionServiceTests : TestBase
    {
        public SecurityTransactionServiceTests(ServiceCollectionFixture serviceCollectionFixture) : base(serviceCollectionFixture)
        {
        }

        [Fact]
        public async Task TestAddAndGetAll()
        {
            var (securityId, brokerAccountId) = await SetupDependencies();

            var dto = new SecurityTransactionDTO
            {
                SecurityId = securityId,
                BrokerAccountId = brokerAccountId,
                Quantity = 10,
                Price = 150m,
                Date = DateTime.UtcNow,
                BrokerCommission = 1.5m,
                StockExchangeCommission = 0.5m,
                IsSell = false
            };

            var addedId = await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<ISecurityTransactionService>();
                return await service.Add(dto);
            });

            Assert.NotEqual(Guid.Empty, addedId);

            var all = await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<ISecurityTransactionService>();
                return await service.GetAll(brokerAccountId, 10, 1);
            });

            Assert.NotNull(all);
            Assert.Contains(all, st => st.Id == addedId && st.Quantity == 10);
        }

        [Fact]
        public async Task TestUpdateAndDelete()
        {
            var (securityId, brokerAccountId) = await SetupDependencies();

            var dto = new SecurityTransactionDTO
            {
                SecurityId = securityId,
                BrokerAccountId = brokerAccountId,
                Quantity = 5,
                Price = 200m,
                Date = DateTime.UtcNow,
                IsSell = false
            };

            var addedId = await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<ISecurityTransactionService>();
                return await service.Add(dto);
            });

            await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<ISecurityTransactionService>();
                await service.Update(new SecurityTransactionDTO
                {
                    Id = addedId,
                    SecurityId = securityId,
                    BrokerAccountId = brokerAccountId,
                    Quantity = 20,
                    Price = 210m,
                    Date = DateTime.UtcNow,
                    IsSell = true
                });
            });

            var all = await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<ISecurityTransactionService>();
                return await service.GetAll(brokerAccountId, 10, 1);
            });

            var updated = all.FirstOrDefault(st => st.Id == addedId);
            Assert.NotNull(updated);
            Assert.Equal(20, updated.Quantity);
            Assert.True(updated.IsSell);

            await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<ISecurityTransactionService>();
                await service.Delete(addedId);
            });

            var listAfterDelete = await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<ISecurityTransactionService>();
                return await service.GetAll(brokerAccountId, 10, 1);
            });

            Assert.DoesNotContain(listAfterDelete, st => st.Id == addedId);
        }

        private async Task<(Guid securityId, Guid brokerAccountId)> SetupDependencies()
        {
            var securityTypeId = await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<ISecurityTypeService>();
                return await service.Add(new SecurityTypeDTO { Name = "SecTx Stock" });
            });

            var securityId = await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<ISecurityService>();
                var sec = await service.Add(new SecurityDTO
                {
                    Name = "Microsoft",
                    Ticker = "MSFT",
                    TypeId = securityTypeId,
                    CurrencyId = CurrencyConstants.USD,
                    ActualPrice = 400m
                }, null);
                return sec.Id;
            });

            var brokerId = await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<IBrokerService>();
                return await service.Add(new BrokerDTO { Name = "SecTx Broker" });
            });

            var brokerAccountTypeId = await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<IBrokerAccountTypeService>();
                return await service.Add(new BrokerAccountTypeDTO { Name = "Standard Broker Acc" });
            });

            var brokerAccountId = await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<IBrokerAccountService>();
                return await service.Add(new BrokerAccountDTO
                {
                    Name = "SecTx Broker Acc",
                    BrokerId = brokerId,
                    TypeId = brokerAccountTypeId,
                    CurrencyId = CurrencyConstants.USD
                });
            });

            return (securityId, brokerAccountId);
        }
    }
}
