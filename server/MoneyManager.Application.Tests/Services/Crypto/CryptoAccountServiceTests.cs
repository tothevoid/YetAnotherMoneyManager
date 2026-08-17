using Microsoft.Extensions.DependencyInjection;
using MoneyManager.Application.Interfaces.Crypto;
using MoneyManager.Application.Tests.Fixtures;
using MoneyManager.Application.DTO.Crypto;
using MoneyManager.Infrastructure.Entities.Crypto;

namespace MoneyManager.Application.Tests.Services.Crypto
{
    public class CryptoAccountServiceTests : TestBase
    {
        public CryptoAccountServiceTests(ServiceCollectionFixture serviceCollectionFixture) : base(serviceCollectionFixture)
        {
        }

        [Fact]
        public async Task TestAddAndGetById()
        {
            var providerId = await CreateProvider("Coinbase");

            var accountId = await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<ICryptoAccountService>();
                return await service.AddAsync(new CryptoAccountDto
                {
                    Name = "Main Crypto Wallet",
                    CryptoProviderId = providerId
                });
            });

            Assert.NotEqual(Guid.Empty, accountId);

            var fetched = await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<ICryptoAccountService>();
                return await service.GetByIdAsync(accountId);
            });

            Assert.NotNull(fetched);
            Assert.Equal(accountId, fetched.Id);
            Assert.Equal("Main Crypto Wallet", fetched.Name);
        }

        [Fact]
        public async Task TestUpdateAndDelete()
        {
            var providerId = await CreateProvider("OKX");

            var accountId = await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<ICryptoAccountService>();
                return await service.AddAsync(new CryptoAccountDto
                {
                    Name = "Initial Account Name",
                    CryptoProviderId = providerId
                });
            });

            await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<ICryptoAccountService>();
                await service.UpdateAsync(new CryptoAccountDto
                {
                    Id = accountId,
                    Name = "Updated Account Name",
                    CryptoProviderId = providerId
                });
            });

            var fetched = await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<ICryptoAccountService>();
                return await service.GetByIdAsync(accountId);
            });

            Assert.NotNull(fetched);
            Assert.Equal("Updated Account Name", fetched.Name);

            await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<ICryptoAccountService>();
                await service.DeleteAsync(accountId);
            });

            var deleted = await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<ICryptoAccountService>();
                return await service.GetByIdAsync(accountId);
            });

            Assert.Null(deleted);
        }

        private async Task<Guid> CreateProvider(string name)
        {
            return await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<ICryptoProviderService>();
                return await service.AddAsync(new CryptoProviderDto { Name = name });
            });
        }
    }
}
