using Microsoft.Extensions.DependencyInjection;
using MoneyManager.Application.DTO.Accounts;
using MoneyManager.Application.DTO.Brokers;
using MoneyManager.Application.Interfaces.Accounts;
using MoneyManager.Application.Interfaces.Brokers;
using MoneyManager.Application.Tests.Fixtures;
using MoneyManager.Infrastructure.Constants;

namespace MoneyManager.Application.Tests.Services.Brokers
{
    public class BrokerAccountFundsTransferServiceTests : TestBase
    {
        public BrokerAccountFundsTransferServiceTests(ServiceCollectionFixture serviceCollectionFixture) : base(serviceCollectionFixture)
        {
        }

        [Fact]
        public async Task TestAddAndGetAll()
        {
            var (brokerAccountId, accountId) = await SetupDependencies();

            var added = await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<IBrokerAccountFundsTransferService>();
                return await service.Add(new BrokerAccountFundsTransferDto
                {
                    BrokerAccountId = brokerAccountId,
                    AccountId = accountId,
                    Amount = 1000m,
                    Income = true,
                    Date = DateTime.UtcNow
                });
            });

            Assert.NotNull(added);
            Assert.NotEqual(Guid.Empty, added.Id);

            var all = await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<IBrokerAccountFundsTransferService>();
                return await service.GetAll();
            });

            Assert.NotNull(all);
            Assert.Contains(all, t => t.Id == added.Id && t.Amount == 1000m);

            var accountTransfers = await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<IBrokerAccountFundsTransferService>();
                return await service.GetAll(brokerAccountId);
            });

            Assert.NotNull(accountTransfers);
            Assert.Contains(accountTransfers, t => t.Id == added.Id);
        }

        [Fact]
        public async Task TestUpdateAndDelete()
        {
            var (brokerAccountId, accountId) = await SetupDependencies();

            var added = await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<IBrokerAccountFundsTransferService>();
                return await service.Add(new BrokerAccountFundsTransferDto
                {
                    BrokerAccountId = brokerAccountId,
                    AccountId = accountId,
                    Amount = 500m,
                    Income = true,
                    Date = DateTime.UtcNow
                });
            });

            await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<IBrokerAccountFundsTransferService>();
                await service.Update(new BrokerAccountFundsTransferDto
                {
                    Id = added.Id,
                    BrokerAccountId = brokerAccountId,
                    AccountId = accountId,
                    Amount = 750m,
                    Income = false,
                    Date = DateTime.UtcNow
                });
            });

            var all = await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<IBrokerAccountFundsTransferService>();
                return await service.GetAll();
            });

            var updated = all.FirstOrDefault(t => t.Id == added.Id);
            Assert.NotNull(updated);
            Assert.Equal(750m, updated.Amount);
            Assert.False(updated.Income);

            await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<IBrokerAccountFundsTransferService>();
                await service.Delete(added.Id);
            });

            var listAfterDelete = await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<IBrokerAccountFundsTransferService>();
                return await service.GetAll();
            });

            Assert.DoesNotContain(listAfterDelete, t => t.Id == added.Id);
        }

        private async Task<(Guid brokerAccountId, Guid accountId)> SetupDependencies()
        {
            var brokerId = await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<IBrokerService>();
                return await service.Add(new BrokerDTO { Name = "Transfer Broker" });
            });

            var typeId = await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<IBrokerAccountTypeService>();
                return await service.Add(new BrokerAccountTypeDTO { Name = "Transfer Acc Type" });
            });

            var brokerAccountId = await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<IBrokerAccountService>();
                return await service.Add(new BrokerAccountDTO
                {
                    Name = "Transfer Broker Acc",
                    BrokerId = brokerId,
                    TypeId = typeId,
                    CurrencyId = CurrencyConstants.USD
                });
            });

            var accountId = await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<IAccountService>();
                return await service.Add(new AccountDTO
                {
                    Active = true,
                    Name = "Transfer Card Acc",
                    AccountTypeId = AccountTypeConstants.Cash,
                    CurrencyId = CurrencyConstants.USD,
                    Balance = 5000m,
                    CreatedOn = DateOnly.FromDateTime(DateTime.Now)
                });
            });

            return (brokerAccountId, accountId);
        }
    }
}
