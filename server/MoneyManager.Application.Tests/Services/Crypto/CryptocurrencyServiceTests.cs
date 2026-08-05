using Microsoft.Extensions.DependencyInjection;
using MoneyManager.Application.Interfaces.Crypto;
using MoneyManager.Application.Tests.Fixtures;
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
    }
}
