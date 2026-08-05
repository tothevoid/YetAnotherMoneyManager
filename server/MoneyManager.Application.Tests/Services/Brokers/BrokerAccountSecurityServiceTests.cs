using Microsoft.Extensions.DependencyInjection;
using MoneyManager.Application.DTO.Brokers;
using MoneyManager.Application.DTO.Securities;
using MoneyManager.Application.Interfaces.Brokers;
using MoneyManager.Application.Interfaces.Securities;
using MoneyManager.Application.Tests.Fixtures;
using MoneyManager.Infrastructure.Constants;

namespace MoneyManager.Application.Tests.Services.Brokers
{
    public class BrokerAccountSecurityServiceTests : TestBase
    {
        public BrokerAccountSecurityServiceTests(ServiceCollectionFixture serviceCollectionFixture) : base(serviceCollectionFixture)
        {
        }

        [Fact]
        public async Task TestAddAndGetAll()
        {
            var (brokerAccountId, securityId) = await SetupDependencies();

            var addedId = await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<IBrokerAccountSecurityService>();
                return await service.Add(new BrokerAccountSecurityDTO
                {
                    BrokerAccountId = brokerAccountId,
                    SecurityId = securityId,
                    Quantity = 50,
                    Price = 100m
                });
            });

            Assert.NotEqual(Guid.Empty, addedId);

            var all = await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<IBrokerAccountSecurityService>();
                return await service.GetAll();
            });

            Assert.NotNull(all);
            Assert.Contains(all, bas => bas.Id == addedId && bas.Quantity == 50);
        }

        [Fact]
        public async Task TestUpdateAndDelete()
        {
            var (brokerAccountId, securityId) = await SetupDependencies();

            var addedId = await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<IBrokerAccountSecurityService>();
                return await service.Add(new BrokerAccountSecurityDTO
                {
                    BrokerAccountId = brokerAccountId,
                    SecurityId = securityId,
                    Quantity = 10,
                    Price = 120m
                });
            });

            await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<IBrokerAccountSecurityService>();
                await service.Update(new BrokerAccountSecurityDTO
                {
                    Id = addedId,
                    BrokerAccountId = brokerAccountId,
                    SecurityId = securityId,
                    Quantity = 30,
                    Price = 120m
                });
            });

            var all = await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<IBrokerAccountSecurityService>();
                return await service.GetAll();
            });

            var updated = all.FirstOrDefault(bas => bas.Id == addedId);
            Assert.NotNull(updated);
            Assert.Equal(30, updated.Quantity);

            await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<IBrokerAccountSecurityService>();
                await service.Delete(addedId);
            });

            var allAfterDelete = await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<IBrokerAccountSecurityService>();
                return await service.GetAll();
            });

            Assert.DoesNotContain(allAfterDelete, bas => bas.Id == addedId);
        }

        private async Task<(Guid brokerAccountId, Guid securityId)> SetupDependencies()
        {
            var brokerId = await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<IBrokerService>();
                return await service.Add(new BrokerDTO { Name = "BAS Broker" });
            });

            var typeId = await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<IBrokerAccountTypeService>();
                return await service.Add(new BrokerAccountTypeDTO { Name = "BAS Account Type" });
            });

            var brokerAccountId = await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<IBrokerAccountService>();
                return await service.Add(new BrokerAccountDTO
                {
                    Name = "BAS Account",
                    BrokerId = brokerId,
                    TypeId = typeId,
                    CurrencyId = CurrencyConstants.USD
                });
            });

            var secTypeId = await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<ISecurityTypeService>();
                return await service.Add(new SecurityTypeDTO { Name = "BAS Security Type" });
            });

            var securityId = await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<ISecurityService>();
                var sec = await service.Add(new SecurityDTO
                {
                    Name = "BAS Security",
                    Ticker = "BASS",
                    TypeId = secTypeId,
                    CurrencyId = CurrencyConstants.USD,
                    ActualPrice = 100m
                }, null);
                return sec.Id;
            });

            return (brokerAccountId, securityId);
        }
    }
}
