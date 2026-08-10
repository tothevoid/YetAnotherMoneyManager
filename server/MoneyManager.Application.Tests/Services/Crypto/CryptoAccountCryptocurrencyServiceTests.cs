using Microsoft.Extensions.DependencyInjection;
using MoneyManager.Application.Interfaces.Crypto;
using MoneyManager.Application.Tests.Fixtures;
using MoneyManager.Application.DTO.Crypto;
using MoneyManager.Infrastructure.Entities.Crypto;

namespace MoneyManager.Application.Tests.Services.Crypto
{
    public class CryptoAccountCryptocurrencyServiceTests : TestBase
    {
        public CryptoAccountCryptocurrencyServiceTests(ServiceCollectionFixture serviceCollectionFixture) : base(serviceCollectionFixture)
        {
        }

        [Fact]
        public async Task TestAddAndGetByCryptoAccount()
        {
            var (accountId, cryptoId) = await SetupDependencies();

            var addedId = await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<ICryptoAccountCryptocurrencyService>();
                return await service.Add(new CryptoAccountCryptocurrencyDto
                {
                    CryptoAccountId = accountId,
                    CryptocurrencyId = cryptoId,
                    Quantity = 2.5m
                });
            });

            Assert.NotEqual(Guid.Empty, addedId);

            var items = await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<ICryptoAccountCryptocurrencyService>();
                return await service.GetByCryptoAccount(accountId);
            });

            Assert.NotNull(items);
            Assert.Contains(items, i => i.Id == addedId && i.Quantity == 2.5m);
        }

        [Fact]
        public async Task TestUpdateAndDelete()
        {
            var (accountId, cryptoId) = await SetupDependencies();

            var addedId = await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<ICryptoAccountCryptocurrencyService>();
                return await service.Add(new CryptoAccountCryptocurrencyDto
                {
                    CryptoAccountId = accountId,
                    CryptocurrencyId = cryptoId,
                    Quantity = 1.0m
                });
            });

            await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<ICryptoAccountCryptocurrencyService>();
                await service.Update(new CryptoAccountCryptocurrencyDto
                {
                    Id = addedId,
                    CryptoAccountId = accountId,
                    CryptocurrencyId = cryptoId,
                    Quantity = 5.0m
                });
            });

            var itemsAfterUpdate = await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<ICryptoAccountCryptocurrencyService>();
                return await service.GetByCryptoAccount(accountId);
            });

            var updated = itemsAfterUpdate.FirstOrDefault(i => i.Id == addedId);
            Assert.NotNull(updated);
            Assert.Equal(5.0m, updated.Quantity);

            await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<ICryptoAccountCryptocurrencyService>();
                await service.Delete(addedId);
            });

            var itemsAfterDelete = await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<ICryptoAccountCryptocurrencyService>();
                return await service.GetByCryptoAccount(accountId);
            });

            Assert.DoesNotContain(itemsAfterDelete, i => i.Id == addedId);
        }

        private async Task<(Guid accountId, Guid cryptoId)> SetupDependencies()
        {
            var providerId = await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<ICryptoProviderService>();
                return await service.Add(new CryptoProviderDto { Name = "Kraken" });
            });

            var accountId = await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<ICryptoAccountService>();
                return await service.Add(new CryptoAccountDto
                {
                    Name = "Kraken Account",
                    CryptoProviderId = providerId
                });
            });

            var crypto = await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<ICryptocurrencyService>();
                return await service.Add(new CryptocurrencyDto { Name = "Solana", Symbol = "SOL", Price = 150m }, null);
            });

            return (accountId, crypto.Id);
        }
    }
}
