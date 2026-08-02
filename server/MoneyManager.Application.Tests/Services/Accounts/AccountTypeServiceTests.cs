using Microsoft.Extensions.DependencyInjection;
using MoneyManager.Application.DTO.Accounts;
using MoneyManager.Application.Interfaces.Accounts;
using MoneyManager.Application.Tests.Fixtures;

namespace MoneyManager.Application.Tests.Services.Accounts
{
    public class AccountTypeServiceTests: TestBase
    {
        public AccountTypeServiceTests(ServiceCollectionFixture serviceCollectionFixture): base(serviceCollectionFixture)
        {
        }

        [Fact]
        public async Task TestAdd()
        {
            var typesBefore = await ExecuteScopeAsync(async sp =>
            {
                var accountTypeService = sp.GetRequiredService<IAccountTypeService>();

                var typesBefore = await accountTypeService.GetAll();

                return typesBefore;
            });

            var names = Enumerable.Range(0, 10).Select(x => $"Account: {x}").ToArray();

            await ExecuteScopeAsync(async sp =>
            {
                var accountTypeService = sp.GetRequiredService<IAccountTypeService>();

                foreach (var name in names)
                {
                    await accountTypeService.Add(new AccountTypeDTO() { Active = true, Id = Guid.NewGuid(), Name = name });
                }
            });

            var typesAfter = await ExecuteScopeAsync(async sp =>
            {
                var accountTypeService = sp.GetRequiredService<IAccountTypeService>();

                var typesAfter = await accountTypeService.GetAll();

                return typesAfter;
            });

            Assert.Equivalent(typesBefore.Count(), typesAfter.Count() - names.Length);
        }

        [Fact]
        public async Task TestUpdate()
        {
            var nameBefore = "TestBefore";

            var id = Guid.NewGuid();

            await ExecuteScopeAsync(async sp =>
            {
                var accountTypeService = sp.GetRequiredService<IAccountTypeService>();

                var accountType = CreateAccountType(id, nameBefore);
                await accountTypeService.Add(accountType);
            });

            var nameAfter = "TestAfter";

            await ExecuteScopeAsync(async sp =>
            {
                var accountTypeService = sp.GetRequiredService<IAccountTypeService>();

                var accountType = CreateAccountType(id, nameAfter);
                await accountTypeService.Update(accountType);
            });

            await ExecuteScopeAsync(async sp =>
            {
                var accountTypeService = sp.GetRequiredService<IAccountTypeService>();

                var typesAfter = await accountTypeService.GetAll();

                var accountTypeWithNameBefore = typesAfter.FirstOrDefault(type => type.Name == nameBefore);

                var accountTypeWithNameAfter = typesAfter.FirstOrDefault(type => type.Name == nameAfter);

                Assert.Null(accountTypeWithNameBefore);
                Assert.NotNull(accountTypeWithNameAfter);
            });
        }

        [Fact]
        public async Task TestDelete()
        {
            var id = Guid.NewGuid();

            await ExecuteScopeAsync(async sp =>
            {
                var accountTypeService = sp.GetRequiredService<IAccountTypeService>();

                var accountType = CreateAccountType(id, nameof(TestDelete));
                await accountTypeService.Add(accountType);
            });

            await ExecuteScopeAsync(async sp =>
            {
                var accountTypeService = sp.GetRequiredService<IAccountTypeService>();

                await accountTypeService.Delete(id);
            });

            await ExecuteScopeAsync(async sp =>
            {
                var accountTypeService = sp.GetRequiredService<IAccountTypeService>();

                var typesAfter = await accountTypeService.GetAll();

                var accountTypesWithSameId = typesAfter.FirstOrDefault(type => type.Id == id);

                Assert.Null(accountTypesWithSameId);
            });
        }

        private AccountTypeDTO CreateAccountType(Guid id, string name) => new() { Id = id, Active = true, Name = name };
    }
}
