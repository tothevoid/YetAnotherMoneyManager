using Microsoft.Extensions.DependencyInjection;
using MoneyManager.Application.Interfaces.Crypto;
using MoneyManager.Application.Tests.Fixtures;
using MoneyManager.Application.DTO.Crypto;
using MoneyManager.Infrastructure.Entities.Crypto;

namespace MoneyManager.Application.Tests.Services.Crypto
{
    public class CryptocurrencyServiceTests : TestBase
    {
        public CryptocurrencyServiceTests(ServiceCollectionFixture serviceCollectionFixture) : base(serviceCollectionFixture)
        {
        }

        [Fact]
        public async Task TestAddAndGetAll()
        {
            var dto = new CryptocurrencyDto
            {
                Name = "Bitcoin",
                Symbol = "BTC",
                Price = 65000m
            };

            var added = await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<ICryptocurrencyService>();
                return await service.Add(dto, null);
            });

            Assert.NotNull(added);
            Assert.NotEqual(Guid.Empty, added.Id);
            Assert.Equal("Bitcoin", added.Name);

            var all = await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<ICryptocurrencyService>();
                return await service.GetAll();
            });

            Assert.NotNull(all);
            Assert.Contains(all, c => c.Id == added.Id && c.Symbol == "BTC");
        }

        [Fact]
        public async Task TestUpdateAndDelete()
        {
            var dto = new CryptocurrencyDto
            {
                Name = "Ethereum",
                Symbol = "ETH",
                Price = 3500m
            };

            var added = await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<ICryptocurrencyService>();
                return await service.Add(dto, null);
            });

            added.Price = 3800m;
            var updated = await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<ICryptocurrencyService>();
                return await service.Update(added, null);
            });

            Assert.NotNull(updated);
            Assert.Equal(3800m, updated.Price);

            await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<ICryptocurrencyService>();
                await service.Delete(added.Id);
            });

            var allAfterDelete = await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<ICryptocurrencyService>();
                return await service.GetAll();
            });

            Assert.DoesNotContain(allAfterDelete, c => c.Id == added.Id);
        }

        [Fact]
        public async Task TestDelete_WithIcon()
        {
            var dto = new CryptocurrencyDto
            {
                Name = "Solana",
                Symbol = "SOL",
                Price = 150m,
                IconKey = "sol-sample-icon"
            };

            var added = await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<ICryptocurrencyService>();
                return await service.Add(dto, null);
            });

            await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<ICryptocurrencyService>();
                await service.Delete(added.Id);
            });

            var all = await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<ICryptocurrencyService>();
                return await service.GetAll();
            });

            Assert.DoesNotContain(all, c => c.Id == added.Id);
        }

        [Fact]
        public async Task TestUpdate_RemoveIcon()
        {
            var dto = new CryptocurrencyDto
            {
                Name = "Cardano",
                Symbol = "ADA",
                Price = 0.5m,
                IconKey = "ada-initial-icon"
            };

            var added = await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<ICryptocurrencyService>();
                return await service.Add(dto, null);
            });

            added.IconKey = null;

            var updated = await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<ICryptocurrencyService>();
                return await service.Update(added, null);
            });

            Assert.NotNull(updated);
            Assert.Null(updated.IconKey);
        }
    }
}
