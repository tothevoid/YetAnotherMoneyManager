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
        public RepositoryTests(PostgresDbFixture fixture) : base(fixture)
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

            await repository.Add(accountType);
            await repository.SaveChanges();

            var fetched = await repository.GetById(accountType.Id);
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

            await repository.Add(accountType);
            await repository.SaveChanges();

            var found = await repository.Find(x => x.Name == uniqueName);
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

            await repository.Add(item1);
            await repository.Add(item2);
            await repository.SaveChanges();

            var activeItems = await repository.GetAll(x => x.Name.StartsWith(prefix) && x.Active);
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

            await repository.Add(item1);
            await repository.Add(item2);
            await repository.Add(item3);
            await repository.SaveChanges();

            var complexQuery = new ComplexQueryBuilder<AccountType>()
                .AddFilter(x => x.Name.StartsWith(prefix))
                .AddOrder(x => x.Name, isDescending: false)
                .AddPagination(pageIndex: 1, recordsQuantity: 2)
                .DisableTracking()
                .GetQuery();

            var results = (await repository.GetAll(complexQuery)).ToList();

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
            await repository.Add(new AccountType { Id = Guid.NewGuid(), Name = prefix + "_1", Active = true });
            await repository.Add(new AccountType { Id = Guid.NewGuid(), Name = prefix + "_2", Active = true });
            await repository.SaveChanges();

            var count = await repository.GetCount(x => x.Name.StartsWith(prefix));
            Assert.Equal(2, count);
        }

        [Fact]
        public async Task Update_ModifiesExistingEntity()
        {
            using var context = CreateDbContext();
            var repository = new Repository<AccountType>(context);

            var item = new AccountType { Id = Guid.NewGuid(), Name = "BeforeUpdate", Active = true };
            await repository.Add(item);
            await repository.SaveChanges();

            item.Name = "AfterUpdate";
            repository.Update(item);
            await repository.SaveChanges();

            var updated = await repository.GetById(item.Id);
            Assert.Equal("AfterUpdate", updated.Name);
        }

        [Fact]
        public async Task Delete_RemovesEntity()
        {
            using var context = CreateDbContext();
            var repository = new Repository<AccountType>(context);

            var item = new AccountType { Id = Guid.NewGuid(), Name = "ToDelete", Active = true };
            await repository.Add(item);
            await repository.SaveChanges();

            await repository.Delete(item.Id);
            await repository.SaveChanges();

            var deleted = await repository.GetById(item.Id);
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

            await repository.Add(item1);
            await repository.Add(item2);
            await repository.SaveChanges();

            var minItem = await repository.GetMin(x => x.Name);
            var maxItem = await repository.GetMax(x => x.Name);

            Assert.NotNull(minItem);
            Assert.NotNull(maxItem);
        }
    }
}
