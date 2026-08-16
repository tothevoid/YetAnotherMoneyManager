using Microsoft.Extensions.DependencyInjection;
using MoneyManager.Application.DTO.Banks;
using MoneyManager.Application.Interfaces.Banks;
using MoneyManager.Application.Tests.Fixtures;

namespace MoneyManager.Application.Tests.Services.Banks
{
    public class BankServiceTests : TestBase
    {
        public BankServiceTests(ServiceCollectionFixture serviceCollectionFixture) : base(serviceCollectionFixture)
        {
        }

        [Fact]
        public async Task TestAddAndGetById()
        {
            var bankId = Guid.NewGuid();
            var name = "JPMorgan Chase";
            var bankDto = new BankDto
            {
                Id = bankId,
                Name = name
            };

            await ExecuteScopeAsync(async sp =>
            {
                var bankService = sp.GetRequiredService<IBankService>();
                await bankService.Add(bankDto, null);
            });

            var fetched = await ExecuteScopeAsync(async sp =>
            {
                var bankService = sp.GetRequiredService<IBankService>();
                return await bankService.GetById(bankId);
            });

            Assert.NotNull(fetched);
            Assert.Equal(bankId, fetched.Id);
            Assert.Equal(name, fetched.Name);
        }

        [Fact]
        public async Task TestGetAll()
        {
            var bankId = Guid.NewGuid();
            var name = "Citigroup";

            await ExecuteScopeAsync(async sp =>
            {
                var bankService = sp.GetRequiredService<IBankService>();
                await bankService.Add(new BankDto { Id = bankId, Name = name }, null);
            });

            var all = await ExecuteScopeAsync(async sp =>
            {
                var bankService = sp.GetRequiredService<IBankService>();
                return await bankService.GetAll();
            });

            Assert.NotNull(all);
            Assert.Contains(all, b => b.Id == bankId && b.Name == name);
        }

        [Fact]
        public async Task TestUpdate()
        {
            var bankId = Guid.NewGuid();
            var name = "Goldman";
            var bankDto = new BankDto
            {
                Id = bankId,
                Name = name
            };
            await ExecuteScopeAsync(async sp =>
            {
                var bankService = sp.GetRequiredService<IBankService>();
                await bankService.Add(bankDto, null);
            });

            await ExecuteScopeAsync(async sp =>
            {
                var bankService = sp.GetRequiredService<IBankService>();
                await bankService.Update(new BankDto { Id = bankId, Name = name + " Sachs" }, null);
            });

            var updated = await ExecuteScopeAsync(async sp =>
            {
                var bankService = sp.GetRequiredService<IBankService>();
                return await bankService.GetById(bankId);
            });

            Assert.NotNull(updated);
            Assert.Equal("Goldman Sachs", updated.Name);
        }

        [Fact]
        public async Task TestDelete()
        {
            var bankId = Guid.NewGuid();
            await ExecuteScopeAsync(async sp =>
            {
                var bankService = sp.GetRequiredService<IBankService>();
                await bankService.Add(new BankDto { Id = bankId, Name = "Morgan Stanley" }, null);
            });

            var result = await ExecuteScopeAsync(async sp =>
            {
                var bankService = sp.GetRequiredService<IBankService>();
                return await bankService.Delete(bankId);
            });

            Assert.True(result);

            var deleted = await ExecuteScopeAsync(async sp =>
            {
                var bankService = sp.GetRequiredService<IBankService>();
                return await bankService.GetById(bankId);
            });

            Assert.Null(deleted);
        }

        [Fact]
        public async Task TestDelete_WithIcon()
        {
            var bankId = Guid.NewGuid();
            await ExecuteScopeAsync(async sp =>
            {
                var bankService = sp.GetRequiredService<IBankService>();
                await bankService.Add(new BankDto { Id = bankId, Name = "Bank With Icon", IconKey = "sample-icon-key" }, null);
            });

            var result = await ExecuteScopeAsync(async sp =>
            {
                var bankService = sp.GetRequiredService<IBankService>();
                return await bankService.Delete(bankId);
            });

            Assert.True(result);

            var deleted = await ExecuteScopeAsync(async sp =>
            {
                var bankService = sp.GetRequiredService<IBankService>();
                return await bankService.GetById(bankId);
            });

            Assert.Null(deleted);
        }

        [Fact]
        public async Task TestUpdate_RemoveIcon()
        {
            var bankId = Guid.NewGuid();
            await ExecuteScopeAsync(async sp =>
            {
                var bankService = sp.GetRequiredService<IBankService>();
                await bankService.Add(new BankDto { Id = bankId, Name = "Bank To Remove Icon", IconKey = "initial-icon-key" }, null);
            });

            await ExecuteScopeAsync(async sp =>
            {
                var bankService = sp.GetRequiredService<IBankService>();
                await bankService.Update(new BankDto { Id = bankId, Name = "Bank To Remove Icon", IconKey = null }, null);
            });

            var updated = await ExecuteScopeAsync(async sp =>
            {
                var bankService = sp.GetRequiredService<IBankService>();
                return await bankService.GetById(bankId);
            });

            Assert.NotNull(updated);
            Assert.Null(updated.IconKey);
        }
    }
}
