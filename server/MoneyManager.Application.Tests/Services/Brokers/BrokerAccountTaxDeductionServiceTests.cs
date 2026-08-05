using Microsoft.Extensions.DependencyInjection;
using MoneyManager.Application.DTO.Brokers;
using MoneyManager.Application.Interfaces.Brokers;
using MoneyManager.Application.Tests.Fixtures;
using MoneyManager.Infrastructure.Constants;

namespace MoneyManager.Application.Tests.Services.Brokers
{
    public class BrokerAccountTaxDeductionServiceTests : TestBase
    {
        public BrokerAccountTaxDeductionServiceTests(ServiceCollectionFixture serviceCollectionFixture) : base(serviceCollectionFixture)
        {
        }

        [Fact]
        public async Task TestAddAndGetAll()
        {
            var brokerAccountId = await SetupBrokerAccount();

            var addedId = await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<IBrokerAccountTaxDeductionService>();
                return await service.Add(new BrokerAccountTaxDeductionDto
                {
                    BrokerAccountId = brokerAccountId,
                    Name = "IIS Tax Deduction 2025",
                    Amount = 52000m,
                    DateApplied = DateTime.UtcNow
                });
            });

            Assert.NotEqual(Guid.Empty, addedId);

            var all = await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<IBrokerAccountTaxDeductionService>();
                return await service.GetAll(brokerAccountId);
            });

            Assert.NotNull(all);
            Assert.Contains(all, d => d.Id == addedId && d.Amount == 52000m);

            var accountAmount = await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<IBrokerAccountTaxDeductionService>();
                return await service.GetAmountByBrokerAccount(brokerAccountId);
            });

            Assert.Equal(52000m, accountAmount);
        }

        [Fact]
        public async Task TestUpdateAndDelete()
        {
            var brokerAccountId = await SetupBrokerAccount();

            var addedId = await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<IBrokerAccountTaxDeductionService>();
                return await service.Add(new BrokerAccountTaxDeductionDto
                {
                    BrokerAccountId = brokerAccountId,
                    Name = "Initial Deduction",
                    Amount = 10000m,
                    DateApplied = DateTime.UtcNow
                });
            });

            await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<IBrokerAccountTaxDeductionService>();
                await service.Update(new BrokerAccountTaxDeductionDto
                {
                    Id = addedId,
                    BrokerAccountId = brokerAccountId,
                    Name = "Updated Deduction",
                    Amount = 15000m,
                    DateApplied = DateTime.UtcNow
                });
            });

            var all = await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<IBrokerAccountTaxDeductionService>();
                return await service.GetAll(brokerAccountId);
            });

            var updated = all.FirstOrDefault(d => d.Id == addedId);
            Assert.NotNull(updated);
            Assert.Equal(15000m, updated.Amount);

            await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<IBrokerAccountTaxDeductionService>();
                await service.Delete(addedId);
            });

            var listAfterDelete = await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<IBrokerAccountTaxDeductionService>();
                return await service.GetAll(brokerAccountId);
            });

            Assert.DoesNotContain(listAfterDelete, d => d.Id == addedId);
        }

        private async Task<Guid> SetupBrokerAccount()
        {
            var brokerId = await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<IBrokerService>();
                return await service.Add(new BrokerDTO { Name = "Deduction Broker" });
            });

            var typeId = await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<IBrokerAccountTypeService>();
                return await service.Add(new BrokerAccountTypeDTO { Name = "IIS Type" });
            });

            return await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<IBrokerAccountService>();
                return await service.Add(new BrokerAccountDTO
                {
                    Name = "IIS Account",
                    BrokerId = brokerId,
                    TypeId = typeId,
                    CurrencyId = CurrencyConstants.USD
                });
            });
        }
    }
}
