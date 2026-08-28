using Audex.Infrastructure.Database;
using Audex.Infrastructure.Entities.Accounts;
using Audex.Infrastructure.Entities.Currencies;
using Audex.Tests.Shared;
using Audex.Tests.Shared.Fixtures;
using Xunit;

namespace Audex.Infrastructure.Tests.Database
{
    public class UnitOfWorkTests : TestBase
    {
        public UnitOfWorkTests(ServiceProviderFixture fixture) : base(fixture)
        {
        }

        [Fact]
        public void CreateRepository_ReturnsRepositoryInstanceAndCachesIt()
        {
            using var context = CreateDbContext();
            var unitOfWork = new UnitOfWork(context);

            var repo1 = unitOfWork.CreateRepository<AccountType>();
            var repo2 = unitOfWork.CreateRepository<AccountType>();
            var currencyRepo = unitOfWork.CreateRepository<Currency>();

            Assert.NotNull(repo1);
            Assert.Same(repo1, repo2);
            Assert.NotNull(currencyRepo);
            Assert.NotSame((object)repo1, (object)currencyRepo);
        }

        [Fact]
        public async Task Commit_PersistsRepositoryChangesToDatabase()
        {
            using var context = CreateDbContext();
            var unitOfWork = new UnitOfWork(context);
            var repository = unitOfWork.CreateRepository<AccountType>();

            var entity = new AccountType
            {
                Id = Guid.NewGuid(),
                Name = "UoWCommitTest_" + Guid.NewGuid().ToString("N"),
                Active = true
            };

            await repository.AddAsync(entity);
            await unitOfWork.CommitAsync();

            using var verifyContext = CreateDbContext();
            var verifyRepo = new Repository<AccountType>(verifyContext);
            var fetched = await verifyRepo.GetByIdAsync(entity.Id);

            Assert.NotNull(fetched);
            Assert.Equal(entity.Name, fetched.Name);
        }

        [Fact]
        public void Dispose_DisposesUnderlyingDbContext()
        {
            var context = CreateDbContext();
            var unitOfWork = new UnitOfWork(context);

            unitOfWork.Dispose(true);

            Assert.Throws<ObjectDisposedException>(() => context.Set<AccountType>().ToList());
        }
    }
}
