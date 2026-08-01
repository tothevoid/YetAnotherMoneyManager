using AutoMapper;
using DocumentFormat.OpenXml.InkML;
using DocumentFormat.OpenXml.Presentation;
using Microsoft.EntityFrameworkCore;
using MoneyManager.Application.DTO.Accounts;
using MoneyManager.Application.Interfaces.Accounts;
using MoneyManager.Application.Mappings;
using MoneyManager.Application.Services.Accounts;
using MoneyManager.Application.Tests.Fixtures;
using MoneyManager.Infrastructure.Database;

namespace MoneyManager.Application.Tests.Services.Accounts
{
    public class AccountTypeServiceTests: IClassFixture<PostgreSqlFixture>, IClassFixture<MapperFixture>
    {
        private readonly PostgreSqlFixture _pgFixture;

        private readonly MapperFixture _mapperFixture;

        public AccountTypeServiceTests(PostgreSqlFixture pgFixture, MapperFixture mapperFixture)
        {
            _pgFixture = pgFixture;
            _mapperFixture = mapperFixture;
        }

        [Fact]
        public async Task TestAdd()
        {
            var uow = new UnitOfWork(_pgFixture.CreateDbContext());

            var accountTypeService = new AccountTypeService(uow, _mapperFixture.Mapper);

            var typesBefore = await accountTypeService.GetAll();

            var names = Enumerable.Range(0, 10).Select(x => $"Account: {x}").ToArray();

            foreach (var name in names)
            {
                await accountTypeService.Add(new AccountTypeDTO() { Active = true, Id = Guid.NewGuid(), Name = name });
            }

            var typesAfter = await accountTypeService.GetAll();

            Assert.Equivalent(typesBefore.Count(), typesAfter.Count() - names.Length);
        }

        [Fact]
        public async Task TestUpdate()
        {
            var nameBefore = "TestBefore";

            var id = Guid.NewGuid();

            await using (var dbArrange = _pgFixture.CreateDbContext())
            {
                var accountTypeService = new AccountTypeService(new UnitOfWork(dbArrange), _mapperFixture.Mapper);
               
                var accountType = CreateAccountType(id, nameBefore);
                await accountTypeService.Add(accountType);
            }

            var nameAfter = "TestAfter";

            await using (var dbAct = _pgFixture.CreateDbContext())
            {
                var accountTypeService = new AccountTypeService(new UnitOfWork(dbAct), _mapperFixture.Mapper);

                var accountType = CreateAccountType(id, nameAfter);

                await accountTypeService.Update(accountType);
            }

            await using (var dbAssert = _pgFixture.CreateDbContext())
            {
                var accountTypeService = new AccountTypeService(new UnitOfWork(dbAssert), _mapperFixture.Mapper);
                var typesAfter = await accountTypeService.GetAll();

                var accountTypeWithNameBefore = typesAfter.FirstOrDefault(type => type.Name == nameBefore);

                var accountTypeWithNameAfter = typesAfter.FirstOrDefault(type => type.Name == nameAfter);

                Assert.Null(accountTypeWithNameBefore);
                Assert.NotNull(accountTypeWithNameAfter);
            }
        }

        [Fact]
        public async Task TestDelete()
        {
            var id = Guid.NewGuid();

            await using (var dbArrange = _pgFixture.CreateDbContext())
            {
                var accountTypeService = new AccountTypeService(new UnitOfWork(dbArrange), _mapperFixture.Mapper);

                var accountType = CreateAccountType(id, nameof(TestDelete));
                await accountTypeService.Add(accountType);
            }

            await using (var dbAct = _pgFixture.CreateDbContext())
            {
                var accountTypeService = new AccountTypeService(new UnitOfWork(dbAct), _mapperFixture.Mapper);

                await accountTypeService.Delete(id);
            }

            await using (var dbAssert = _pgFixture.CreateDbContext())
            {
                var accountTypeService = new AccountTypeService(new UnitOfWork(dbAssert), _mapperFixture.Mapper);
                var typesAfter = await accountTypeService.GetAll();

                var accountTypesWithSameId = typesAfter.FirstOrDefault(type => type.Id == id);

                Assert.Null(accountTypesWithSameId);
            }
        }


        private AccountTypeDTO CreateAccountType(Guid id, string name) => new() { Id = id, Active = true, Name = name };
    }
}
