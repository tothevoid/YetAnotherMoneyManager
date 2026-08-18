using Microsoft.Extensions.DependencyInjection;
using MoneyManager.Application.Interfaces.Crypto;
using MoneyManager.Application.Tests.Fixtures;
using MoneyManager.Application.DTO.Crypto;
using MoneyManager.Infrastructure.Entities.Crypto;

namespace MoneyManager.Application.Tests.Services.Crypto
{
    public class CryptocurrencyServiceTests : TestBase
    {
        public CryptocurrencyServiceTests(ServiceProviderFixture serviceProviderFixture) : base(serviceProviderFixture)
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
                return await service.AddAsync(dto, null);
            });

            Assert.NotNull(added);
            Assert.NotEqual(Guid.Empty, added.Id);
            Assert.Equal("Bitcoin", added.Name);

            var all = await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<ICryptocurrencyService>();
                return await service.GetAllAsync();
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
                return await service.AddAsync(dto, null);
            });

            added.Price = 3800m;
            var updated = await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<ICryptocurrencyService>();
                return await service.UpdateAsync(added, null);
            });

            Assert.NotNull(updated);
            Assert.Equal(3800m, updated.Price);

            await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<ICryptocurrencyService>();
                await service.DeleteAsync(added.Id);
            });

            var allAfterDelete = await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<ICryptocurrencyService>();
                return await service.GetAllAsync();
            });

            Assert.DoesNotContain(allAfterDelete, c => c.Id == added.Id);
        }

        [Fact]
        [Trait("Category", "S3")]
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
                return await service.AddAsync(dto, null);
            });

            await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<ICryptocurrencyService>();
                await service.DeleteAsync(added.Id);
            });

            var all = await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<ICryptocurrencyService>();
                return await service.GetAllAsync();
            });

            Assert.DoesNotContain(all, c => c.Id == added.Id);
        }

        [Fact]
        [Trait("Category", "S3")]
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
                return await service.AddAsync(dto, null);
            });

            added.IconKey = null;

            var updated = await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<ICryptocurrencyService>();
                return await service.UpdateAsync(added, null);
            });

            Assert.NotNull(updated);
            Assert.Null(updated.IconKey);
        }

        [Fact]
        [Trait("Category", "S3")]
        public async Task TestAdd_WithIcon_GeneratesVersionedKey()
        {
            var formFile = CreateDummyFormFile();
            var dto = new CryptocurrencyDto
            {
                Name = "Polkadot",
                Symbol = "DOT",
                Price = 7m
            };

            var added = await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<ICryptocurrencyService>();
                return await service.AddAsync(dto, formFile);
            });

            Assert.NotNull(added.IconKey);
            Assert.StartsWith(added.Id.ToString(), added.IconKey);
            Assert.NotEqual(added.Id.ToString(), added.IconKey);
        }

        [Fact]
        [Trait("Category", "S3")]
        public async Task TestUpdate_ReplaceIcon_GeneratesNewKey()
        {
            var formFile1 = CreateDummyFormFile();
            var formFile2 = CreateDummyFormFile();

            var dto = new CryptocurrencyDto
            {
                Name = "Avalanche",
                Symbol = "AVAX",
                Price = 30m
            };

            var added = await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<ICryptocurrencyService>();
                return await service.AddAsync(dto, formFile1);
            });

            var initialKey = added.IconKey;
            Assert.NotNull(initialKey);

            var updated = await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<ICryptocurrencyService>();
                return await service.UpdateAsync(added, formFile2);
            });

            Assert.NotNull(updated.IconKey);
            Assert.NotEqual(initialKey, updated.IconKey);
            Assert.StartsWith(added.Id.ToString(), updated.IconKey);

            var iconStream = await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<ICryptocurrencyService>();
                return await service.GetIconStreamAsync(updated.IconKey);
            });

            Assert.NotNull(iconStream);
            Assert.NotNull(iconStream.Stream);
            Assert.Equal("image/png", iconStream.ContentType);
        }

        private static Microsoft.AspNetCore.Http.IFormFile CreateDummyFormFile()
        {
            var content = System.Text.Encoding.UTF8.GetBytes("dummy crypto image");
            return new Microsoft.AspNetCore.Http.FormFile(new System.IO.MemoryStream(content), 0, content.Length, "icon", "icon.png")
            {
                Headers = new Microsoft.AspNetCore.Http.HeaderDictionary(),
                ContentType = "image/png"
            };
        }
    }
}
