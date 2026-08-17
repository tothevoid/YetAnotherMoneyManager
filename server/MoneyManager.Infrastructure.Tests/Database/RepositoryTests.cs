using MoneyManager.Infrastructure.Database;
using MoneyManager.Infrastructure.Entities.Accounts;
using MoneyManager.Infrastructure.Queries;
using MoneyManager.Tests.Shared;
using MoneyManager.Tests.Shared.Fixtures;
using Xunit;

namespace MoneyManager.Infrastructure.Tests.Database
{
    public class RepositoryTests : TestBase
    {
        public RepositoryTests(ServiceProviderFixture fixture) : base(fixture)
        {
        }

        [Fact]
        public async Task Add_And_GetById_ReturnsAddedEntity()
        {
            using var context = CreateDbContext();
            var repository = new Repository<AccountType>(context);

            var accountType = new AccountType
            {
                Id = Guid.NewGuid(),
                Name = "Test Account Type " + Guid.NewGuid().ToString("N"),
                Active = true
            };

            await repository.AddAsync(accountType);
            await repository.SaveChangesAsync();

            var fetched = await repository.GetByIdAsync(accountType.Id);
            Assert.NotNull(fetched);
            Assert.Equal(accountType.Name, fetched.Name);
        }

        [Fact]
        public async Task Find_ReturnsMatchingEntity()
        {
            using var context = CreateDbContext();
            var repository = new Repository<AccountType>(context);

            var uniqueName = "FindTest_" + Guid.NewGuid().ToString("N");
            var accountType = new AccountType
            {
                Id = Guid.NewGuid(),
                Name = uniqueName,
                Active = true
            };

            await repository.AddAsync(accountType);
            await repository.SaveChangesAsync();

            var found = await repository.FindAsync(x => x.Name == uniqueName);
            Assert.NotNull(found);
            Assert.Equal(accountType.Id, found.Id);
        }

        [Fact]
        public async Task GetAll_WithFilter_ReturnsFilteredEntities()
        {
            using var context = CreateDbContext();
            var repository = new Repository<AccountType>(context);

            var prefix = "GetAllFilter_" + Guid.NewGuid().ToString("N");
            var item1 = new AccountType { Id = Guid.NewGuid(), Name = prefix + "_1", Active = true };
            var item2 = new AccountType { Id = Guid.NewGuid(), Name = prefix + "_2", Active = false };

            await repository.AddAsync(item1);
            await repository.AddAsync(item2);
            await repository.SaveChangesAsync();

            var activeItems = await repository.GetAllAsync(x => x.Name.StartsWith(prefix) && x.Active);
            Assert.Single(activeItems);
            Assert.Equal(item1.Id, activeItems.First().Id);
        }

        [Fact]
        public async Task GetAll_WithComplexQuery_AppliesFilterOrderingPagination()
        {
            using var context = CreateDbContext();
            var repository = new Repository<AccountType>(context);

            var prefix = "Complex_" + Guid.NewGuid().ToString("N");
            var item1 = new AccountType { Id = Guid.NewGuid(), Name = prefix + "_B", Active = true };
            var item2 = new AccountType { Id = Guid.NewGuid(), Name = prefix + "_A", Active = true };
            var item3 = new AccountType { Id = Guid.NewGuid(), Name = prefix + "_C", Active = true };

            await repository.AddAsync(item1);
            await repository.AddAsync(item2);
            await repository.AddAsync(item3);
            await repository.SaveChangesAsync();

            var complexQuery = new ComplexQueryBuilder<AccountType>()
                .AddFilter(x => x.Name.StartsWith(prefix))
                .AddOrder(x => x.Name, isDescending: false)
                .AddPagination(pageIndex: 1, recordsQuantity: 2)
                .DisableTracking()
                .GetQuery();

            var results = (await repository.GetAllAsync(complexQuery)).ToList();

            Assert.Equal(2, results.Count);
            Assert.Equal(item2.Name, results[0].Name); // "Complex_..._A" comes first
            Assert.Equal(item1.Name, results[1].Name); // "Complex_..._B" comes second
        }

        [Fact]
        public async Task GetCount_ReturnsCorrectCount()
        {
            using var context = CreateDbContext();
            var repository = new Repository<AccountType>(context);

            var prefix = "Count_" + Guid.NewGuid().ToString("N");
            await repository.AddAsync(new AccountType { Id = Guid.NewGuid(), Name = prefix + "_1", Active = true });
            await repository.AddAsync(new AccountType { Id = Guid.NewGuid(), Name = prefix + "_2", Active = true });
            await repository.SaveChangesAsync();

            var count = await repository.GetCountAsync(x => x.Name.StartsWith(prefix));
            Assert.Equal(2, count);
        }

        [Fact]
        public async Task Update_ModifiesExistingEntity()
        {
            using var context = CreateDbContext();
            var repository = new Repository<AccountType>(context);

            var item = new AccountType { Id = Guid.NewGuid(), Name = "BeforeUpdate", Active = true };
            await repository.AddAsync(item);
            await repository.SaveChangesAsync();

            item.Name = "AfterUpdate";
            repository.Update(item);
            await repository.SaveChangesAsync();

            var updated = await repository.GetByIdAsync(item.Id);
            Assert.Equal("AfterUpdate", updated.Name);
        }

        [Fact]
        public async Task Delete_RemovesEntity()
        {
            using var context = CreateDbContext();
            var repository = new Repository<AccountType>(context);

            var item = new AccountType { Id = Guid.NewGuid(), Name = "ToDelete", Active = true };
            await repository.AddAsync(item);
            await repository.SaveChangesAsync();

            await repository.DeleteAsync(item.Id);
            await repository.SaveChangesAsync();

            var deleted = await repository.GetByIdAsync(item.Id);
            Assert.Null(deleted);
        }

        [Fact]
        public async Task GetMin_And_GetMax_ReturnsBoundaryEntities()
        {
            using var context = CreateDbContext();
            var repository = new Repository<AccountType>(context);

            var prefix = "MinMax_" + Guid.NewGuid().ToString("N");
            var item1 = new AccountType { Id = Guid.NewGuid(), Name = prefix + "_10", Active = true };
            var item2 = new AccountType { Id = Guid.NewGuid(), Name = prefix + "_50", Active = true };

            await repository.AddAsync(item1);
            await repository.AddAsync(item2);
            await repository.SaveChangesAsync();

            var minItem = await repository.GetMinAsync(x => x.Name);
            var maxItem = await repository.GetMaxAsync(x => x.Name);

            Assert.NotNull(minItem);
            Assert.NotNull(maxItem);
        }
    }
}
