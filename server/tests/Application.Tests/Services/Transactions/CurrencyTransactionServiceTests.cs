using Microsoft.Extensions.DependencyInjection;
using Audex.Application.DTO.Accounts;
using Audex.Application.DTO.Transactions;
using Audex.Application.Interfaces.Accounts;
using Audex.Application.Interfaces.Transactions;
using Audex.Application.Tests.Fixtures;
using Audex.Infrastructure.Constants;

namespace Audex.Application.Tests.Services.Transactions
{
    public class CurrencyTransactionServiceTests : TestBase
    {
        public CurrencyTransactionServiceTests(ServiceProviderFixture serviceProviderFixture) : base(serviceProviderFixture)
        {
        }

        [Fact]
        public async Task TestAddAndGetById()
        {
            var (sourceId, destId) = await SetupAccounts();

            var dto = new CurrencyTransactionDto
            {
                Name = "USD to EUR Exchange",
                SourceAccountId = sourceId,
                DestinationAccountId = destId,
                Amount = 1000m,
                Rate = 0.85m,
                Date = DateOnly.FromDateTime(DateTime.Now)
            };

            var id = await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<ICurrencyTransactionService>();
                return await service.AddAsync(dto);
            });

            Assert.NotEqual(Guid.Empty, id);

            var fetched = await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<ICurrencyTransactionService>();
                return await service.GetByIdAsync(id);
            });

            Assert.NotNull(fetched);
            Assert.Equal("USD to EUR Exchange", fetched.Name);
            Assert.Equal(1000m, fetched.Amount);
            Assert.Equal(0.85m, fetched.Rate);
        }

        [Fact]
        public async Task TestGetAllByAccountId()
        {
            var (sourceId, destId) = await SetupAccounts();

            var dto = new CurrencyTransactionDto
            {
                Name = "Fx Transfer",
                SourceAccountId = sourceId,
                DestinationAccountId = destId,
                Amount = 500m,
                Rate = 0.9m,
                Date = DateOnly.FromDateTime(DateTime.Now)
            };

            var id = await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<ICurrencyTransactionService>();
                return await service.AddAsync(dto);
            });

            var sourceTransactions = await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<ICurrencyTransactionService>();
                return await service.GetAllByAccountIdAsync(sourceId);
            });

            Assert.NotNull(sourceTransactions);
            Assert.Contains(sourceTransactions, t => t.Id == id);

            var destTransactions = await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<ICurrencyTransactionService>();
                return await service.GetAllByAccountIdAsync(destId);
            });

            Assert.NotNull(destTransactions);
            Assert.Contains(destTransactions, t => t.Id == id);
        }

        [Fact]
        public async Task TestUpdate()
        {
            var (sourceId, destId) = await SetupAccounts();

            var id = await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<ICurrencyTransactionService>();
                return await service.AddAsync(new CurrencyTransactionDto
                {
                    Name = "Initial Fx",
                    SourceAccountId = sourceId,
                    DestinationAccountId = destId,
                    Amount = 200m,
                    Rate = 1.0m,
                    Date = DateOnly.FromDateTime(DateTime.Now)
                });
            });

            await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<ICurrencyTransactionService>();
                var item = await service.GetByIdAsync(id);
                item.Name = "Updated Fx";
                item.Amount = 300m;
                item.SourceAccount = null;
                item.DestinationAccount = null;
                await service.UpdateAsync(item);
            });

            var updated = await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<ICurrencyTransactionService>();
                return await service.GetByIdAsync(id);
            });

            Assert.NotNull(updated);
            Assert.Equal("Updated Fx", updated.Name);
            Assert.Equal(300m, updated.Amount);
        }

        [Fact]
        public async Task TestDelete()
        {
            var (sourceId, destId) = await SetupAccounts();

            var id = await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<ICurrencyTransactionService>();
                return await service.AddAsync(new CurrencyTransactionDto
                {
                    Name = "To Delete Fx",
                    SourceAccountId = sourceId,
                    DestinationAccountId = destId,
                    Amount = 100m,
                    Rate = 1.0m,
                    Date = DateOnly.FromDateTime(DateTime.Now)
                });
            });

            await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<ICurrencyTransactionService>();
                await service.DeleteAsync(id);
            });

            var deleted = await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<ICurrencyTransactionService>();
                return await service.GetByIdAsync(id);
            });

            Assert.Null(deleted);
        }

        [Fact]
        public async Task TestGetSummaryByAccountId()
        {
            var (sourceId, destId) = await SetupAccounts();

            await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<ICurrencyTransactionService>();
                await service.AddAsync(new CurrencyTransactionDto
                {
                    Name = "FX 1",
                    SourceAccountId = sourceId,
                    DestinationAccountId = destId,
                    Amount = 1000m,
                    Rate = 1.10m,
                    Date = DateOnly.FromDateTime(DateTime.Now)
                });
                await service.AddAsync(new CurrencyTransactionDto
                {
                    Name = "FX 2",
                    SourceAccountId = sourceId,
                    DestinationAccountId = destId,
                    Amount = 500m,
                    Rate = 1.05m,
                    Date = DateOnly.FromDateTime(DateTime.Now)
                });
            });

            var summary = await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<ICurrencyTransactionService>();
                return await service.GetSummaryByAccountIdAsync(sourceId);
            });

            Assert.NotNull(summary);
            Assert.Equal(2, summary.TransactionsCount);
        }

        [Fact]
        public async Task TestGetPaginationAndSorting()
        {
            var (sourceId, destId) = await SetupAccounts();

            await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<ICurrencyTransactionService>();
                await service.AddAsync(new CurrencyTransactionDto
                {
                    Name = "FX Older",
                    SourceAccountId = sourceId,
                    DestinationAccountId = destId,
                    Amount = 100m,
                    Rate = 1.0m,
                    Date = new DateOnly(2025, 1, 1)
                });
                await service.AddAsync(new CurrencyTransactionDto
                {
                    Name = "FX Newer",
                    SourceAccountId = sourceId,
                    DestinationAccountId = destId,
                    Amount = 200m,
                    Rate = 1.0m,
                    Date = new DateOnly(2025, 6, 1)
                });
            });

            var pagination = await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<ICurrencyTransactionService>();
                return await service.GetPaginationAsync(sourceId);
            });

            Assert.NotNull(pagination);
            Assert.Equal(2, pagination.RecordsQuantity);

            var page1 = await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<ICurrencyTransactionService>();
                return await service.GetAllByAccountIdAsync(sourceId, 1, 1);
            });

            Assert.Single(page1);
            Assert.Equal("FX Newer", page1.First().Name);
        }

        private async Task<(Guid sourceId, Guid destId)> SetupAccounts()
        {
            var sourceId = await ExecuteScopeAsync(async sp =>
            {
                var accService = sp.GetRequiredService<IAccountService>();
                return await accService.AddAsync(new AccountDto
                {
                    Active = true,
                    Name = "Fx Source Acc",
                    AccountTypeId = AccountTypeConstants.Cash,
                    CurrencyId = CurrencyConstants.USD,
                    Balance = 5000m,
                    CreatedOn = DateOnly.FromDateTime(DateTime.Now)
                });
            });

            var destId = await ExecuteScopeAsync(async sp =>
            {
                var accService = sp.GetRequiredService<IAccountService>();
                return await accService.AddAsync(new AccountDto
                {
                    Active = true,
                    Name = "Fx Dest Acc",
                    AccountTypeId = AccountTypeConstants.Cash,
                    CurrencyId = CurrencyConstants.EUR,
                    Balance = 2000m,
                    CreatedOn = DateOnly.FromDateTime(DateTime.Now)
                });
            });

            return (sourceId, destId);
        }
    }
}
