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
        public BrokerAccountFundsTransferServiceTests(ServiceProviderFixture serviceProviderFixture) : base(serviceProviderFixture)
        {
        }

        [Fact]
        public async Task TestAddAndGetAll()
        {
            var (brokerAccountId, accountId) = await SetupDependencies();

            var added = await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<IBrokerAccountFundsTransferService>();
                return await service.AddAsync(new BrokerAccountFundsTransferDto
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
                return await service.GetAllAsync();
            });

            Assert.NotNull(all);
            Assert.Contains(all, t => t.Id == added.Id && t.Amount == 1000m);

            var accountTransfers = await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<IBrokerAccountFundsTransferService>();
                return await service.GetAllAsync(brokerAccountId);
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
                return await service.AddAsync(new BrokerAccountFundsTransferDto
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
                await service.UpdateAsync(new BrokerAccountFundsTransferDto
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
                return await service.GetAllAsync();
            });

            var updated = all.FirstOrDefault(t => t.Id == added.Id);
            Assert.NotNull(updated);
            Assert.Equal(750m, updated.Amount);
            Assert.False(updated.Income);

            await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<IBrokerAccountFundsTransferService>();
                await service.DeleteAsync(added.Id);
            });

            var listAfterDelete = await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<IBrokerAccountFundsTransferService>();
                return await service.GetAllAsync();
            });

            Assert.DoesNotContain(listAfterDelete, t => t.Id == added.Id);
        }

        [Fact]
        public async Task TestGetSumTillSpecificDate()
        {
            var (broker1AccId, account1Id) = await SetupDependencies();
            var (broker2AccId, account2Id) = await SetupDependencies();
            var targetDate = new DateOnly(2020, 1, 15);

            // Broker 1 transfers
            await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<IBrokerAccountFundsTransferService>();
                // Deposit before date
                await service.AddAsync(new BrokerAccountFundsTransferDto
                {
                    BrokerAccountId = broker1AccId,
                    AccountId = account1Id,
                    Amount = 1000m,
                    Income = true,
                    Date = new DateTime(2020, 1, 10, 10, 0, 0, DateTimeKind.Utc)
                });
            });

            await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<IBrokerAccountFundsTransferService>();
                // Withdraw before date
                await service.AddAsync(new BrokerAccountFundsTransferDto
                {
                    BrokerAccountId = broker1AccId,
                    AccountId = account1Id,
                    Amount = 300m,
                    Income = false,
                    Date = new DateTime(2020, 1, 12, 10, 0, 0, DateTimeKind.Utc)
                });
            });

            await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<IBrokerAccountFundsTransferService>();
                // Deposit after target date (boundary out)
                await service.AddAsync(new BrokerAccountFundsTransferDto
                {
                    BrokerAccountId = broker1AccId,
                    AccountId = account1Id,
                    Amount = 500m,
                    Income = true,
                    Date = new DateTime(2020, 1, 20, 10, 0, 0, DateTimeKind.Utc)
                });
            });

            // Broker 2 transfers
            await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<IBrokerAccountFundsTransferService>();
                // Deposit before date
                await service.AddAsync(new BrokerAccountFundsTransferDto
                {
                    BrokerAccountId = broker2AccId,
                    AccountId = account2Id,
                    Amount = 2000m,
                    Income = true,
                    Date = new DateTime(2020, 1, 5, 10, 0, 0, DateTimeKind.Utc)
                });
            });

            await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<IBrokerAccountFundsTransferService>();
                // Withdraw before date
                await service.AddAsync(new BrokerAccountFundsTransferDto
                {
                    BrokerAccountId = broker2AccId,
                    AccountId = account2Id,
                    Amount = 700m,
                    Income = false,
                    Date = new DateTime(2020, 1, 14, 10, 0, 0, DateTimeKind.Utc)
                });
            });

            await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<IBrokerAccountFundsTransferService>();
                // Withdraw after target date (boundary out)
                await service.AddAsync(new BrokerAccountFundsTransferDto
                {
                    BrokerAccountId = broker2AccId,
                    AccountId = account2Id,
                    Amount = 400m,
                    Income = false,
                    Date = new DateTime(2020, 1, 25, 10, 0, 0, DateTimeKind.Utc)
                });
            });

            // Verify Broker 1 sum till targetDate
            var (depositedB1, withdrawnB1) = await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<IBrokerAccountFundsTransferService>();
                return await service.GetSumTillSpecificDateAsync(targetDate, broker1AccId);
            });
            Assert.Equal(1000m, depositedB1);
            Assert.Equal(300m, withdrawnB1);

            // Verify Broker 2 sum till targetDate
            var (depositedB2, withdrawnB2) = await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<IBrokerAccountFundsTransferService>();
                return await service.GetSumTillSpecificDateAsync(targetDate, broker2AccId);
            });
            Assert.Equal(2000m, depositedB2);
            Assert.Equal(700m, withdrawnB2);

            // Verify All brokers sum till targetDate
            var (depositedAll, withdrawnAll) = await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<IBrokerAccountFundsTransferService>();
                return await service.GetSumTillSpecificDateAsync(targetDate, null);
            });
            Assert.Equal(3000m, depositedAll);
            Assert.Equal(1000m, withdrawnAll);
        }

        private async Task<(Guid brokerAccountId, Guid accountId)> SetupDependencies()
        {
            var brokerId = await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<IBrokerService>();
                return await service.AddAsync(new BrokerDto { Name = "Transfer Broker" });
            });

            var typeId = await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<IBrokerAccountTypeService>();
                return await service.AddAsync(new BrokerAccountTypeDto { Name = "Transfer Acc Type" });
            });

            var brokerAccountId = await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<IBrokerAccountService>();
                return await service.AddAsync(new BrokerAccountDto
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
                return await service.AddAsync(new AccountDto
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
