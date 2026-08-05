using Microsoft.Extensions.DependencyInjection;
using MoneyManager.Application.DTO.Transactions;
using MoneyManager.Application.Interfaces.Transactions;
using MoneyManager.Application.Tests.Fixtures;

namespace MoneyManager.Application.Tests.Services.Transactions
{
    public class TransactionTypeServiceTests : TestBase
    {
        public TransactionTypeServiceTests(ServiceCollectionFixture serviceCollectionFixture) : base(serviceCollectionFixture)
        {
        }

        [Fact]
        public async Task TestGetAll_ReturnsTransactionTypes()
        {
            var types = await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<ITransactionTypeService>();
                return await service.GetAll(false);
            });

            Assert.NotNull(types);
            Assert.NotEmpty(types);
        }

        [Fact]
        public async Task TestAdd_CreatesTransactionType()
        {
            var dto = new TransactionTypeDTO
            {
                Id = Guid.NewGuid(),
                Name = "Test Expense",
                Active = true
            };

            var added = await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<ITransactionTypeService>();
                return await service.Add(dto, null);
            });

            Assert.NotNull(added);
            Assert.NotEqual(Guid.Empty, added.Id);
            Assert.Equal("Test Expense", added.Name);

            var all = await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<ITransactionTypeService>();
                return await service.GetAll(false);
            });

            Assert.Contains(all, t => t.Id == added.Id && t.Name == "Test Expense");
        }

        [Fact]
        public async Task TestUpdate_ModifiesTransactionType()
        {
            var dto = new TransactionTypeDTO
            {
                Id = Guid.NewGuid(),
                Name = "Original Type",
                Active = true
            };

            var added = await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<ITransactionTypeService>();
                return await service.Add(dto, null);
            });

            added.Name = "Updated Type";

            var updated = await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<ITransactionTypeService>();
                return await service.Update(added, null);
            });

            Assert.NotNull(updated);
            Assert.Equal("Updated Type", updated.Name);

            var all = await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<ITransactionTypeService>();
                return await service.GetAll(false);
            });

            Assert.Contains(all, t => t.Id == added.Id && t.Name == "Updated Type");
        }

        [Fact]
        public async Task TestDelete_RemovesTransactionType()
        {
            var dto = new TransactionTypeDTO
            {
                Id = Guid.NewGuid(),
                Name = "To Delete Type",
                Active = true
            };

            var added = await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<ITransactionTypeService>();
                return await service.Add(dto, null);
            });

            await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<ITransactionTypeService>();
                await service.Delete(added.Id);
            });

            var all = await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<ITransactionTypeService>();
                return await service.GetAll(false);
            });

            Assert.DoesNotContain(all, t => t.Id == added.Id);
        }

        [Fact]
        public async Task TestGetAll_OnlyActiveFiltering()
        {
            var activeType = await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<ITransactionTypeService>();
                return await service.Add(new TransactionTypeDTO { Name = "Active Type", Active = true }, null);
            });

            var inactiveType = await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<ITransactionTypeService>();
                return await service.Add(new TransactionTypeDTO { Name = "Inactive Type", Active = false }, null);
            });

            var onlyActive = await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<ITransactionTypeService>();
                return await service.GetAll(true);
            });

            Assert.Contains(onlyActive, t => t.Id == activeType.Id);
            Assert.DoesNotContain(onlyActive, t => t.Id == inactiveType.Id);
        }
    }
}
