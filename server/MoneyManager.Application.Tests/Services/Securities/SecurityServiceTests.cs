using Microsoft.Extensions.DependencyInjection;
using MoneyManager.Application.DTO.Securities;
using MoneyManager.Application.Interfaces.Securities;
using MoneyManager.Application.Tests.Fixtures;
using MoneyManager.Infrastructure.Constants;

namespace MoneyManager.Application.Tests.Services.Securities
{
    public class SecurityServiceTests : TestBase
    {
        public SecurityServiceTests(ServiceCollectionFixture serviceCollectionFixture) : base(serviceCollectionFixture)
        {
        }

        [Fact]
        public async Task TestAddAndGetAll()
        {
            var typeId = await CreateSecurityType("Equity");

            var dto = new SecurityDTO
            {
                Name = "Apple Inc.",
                Ticker = "AAPL",
                TypeId = typeId,
                CurrencyId = CurrencyConstants.USD,
                ActualPrice = 220m
            };

            var added = await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<ISecurityService>();
                return await service.Add(dto, null);
            });

            Assert.NotNull(added);
            Assert.NotEqual(Guid.Empty, added.Id);
            Assert.Equal("AAPL", added.Ticker);

            var all = await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<ISecurityService>();
                return await service.GetAll();
            });

            Assert.NotNull(all);
            Assert.Contains(all, s => s.Id == added.Id && s.Ticker == "AAPL");
        }

        [Fact]
        public async Task TestUpdateAndDelete()
        {
            var typeId = await CreateSecurityType("ETF");

            var dto = new SecurityDTO
            {
                Name = "S&P 500 ETF",
                Ticker = "VOO",
                TypeId = typeId,
                CurrencyId = CurrencyConstants.USD,
                ActualPrice = 450m
            };

            var added = await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<ISecurityService>();
                return await service.Add(dto, null);
            });

            added.ActualPrice = 480m;
            var updated = await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<ISecurityService>();
                return await service.Update(added, null);
            });

            Assert.NotNull(updated);
            Assert.Equal(480m, updated.ActualPrice);

            await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<ISecurityService>();
                await service.Delete(added.Id);
            });

            var allAfterDelete = await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<ISecurityService>();
                return await service.GetAll();
            });

            Assert.DoesNotContain(allAfterDelete, s => s.Id == added.Id);
        }

        [Fact]
        public async Task TestFindByTickerAndFindByTickers()
        {
            var typeId = await CreateSecurityType("Equity");

            var dto = new SecurityDTO
            {
                Name = "Microsoft Corp.",
                Ticker = "MSFT",
                TypeId = typeId,
                CurrencyId = CurrencyConstants.USD,
                ActualPrice = 400m
            };

            await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<ISecurityService>();
                await service.Add(dto, null);
            });

            var found = await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<ISecurityService>();
                return await service.FindByTicker("msft");
            });

            Assert.NotNull(found);
            Assert.Equal("MSFT", found.Ticker);

            var foundList = await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<ISecurityService>();
                return await service.FindByTickers(["MSFT", "NONEXISTENT"]);
            });

            Assert.Single(foundList);
            Assert.Equal("MSFT", foundList.First().Ticker);
        }

        [Fact]
        public async Task TestDelete_WithIcon()
        {
            var typeId = await CreateSecurityType("EquityIcon");

            var dto = new SecurityDTO
            {
                Name = "Security With Icon",
                Ticker = "SWI",
                TypeId = typeId,
                CurrencyId = CurrencyConstants.USD,
                ActualPrice = 100m,
                IconKey = "security-sample-icon"
            };

            var added = await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<ISecurityService>();
                return await service.Add(dto, null);
            });

            await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<ISecurityService>();
                await service.Delete(added.Id);
            });

            var all = await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<ISecurityService>();
                return await service.GetAll();
            });

            Assert.DoesNotContain(all, s => s.Id == added.Id);
        }

        [Fact]
        public async Task TestUpdate_RemoveIcon()
        {
            var typeId = await CreateSecurityType("EquityIcon2");

            var dto = new SecurityDTO
            {
                Name = "Security To Remove Icon",
                Ticker = "SRI",
                TypeId = typeId,
                CurrencyId = CurrencyConstants.USD,
                ActualPrice = 150m,
                IconKey = "security-initial-icon"
            };

            var added = await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<ISecurityService>();
                return await service.Add(dto, null);
            });

            added.IconKey = null;

            var updated = await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<ISecurityService>();
                return await service.Update(added, null);
            });

            Assert.NotNull(updated);
            Assert.Null(updated.IconKey);
        }

        [Fact]
        public async Task TestAdd_WithIcon_GeneratesVersionedKey()
        {
            var typeId = await CreateSecurityType("EquityIcon3");
            var formFile = CreateDummyFormFile();

            var dto = new SecurityDTO
            {
                Name = "Security Versioned Icon",
                Ticker = "SVI",
                TypeId = typeId,
                CurrencyId = CurrencyConstants.USD,
                ActualPrice = 200m
            };

            var added = await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<ISecurityService>();
                return await service.Add(dto, formFile);
            });

            Assert.NotNull(added.IconKey);
            Assert.StartsWith(added.Id.ToString(), added.IconKey);
            Assert.NotEqual(added.Id.ToString(), added.IconKey);
        }

        [Fact]
        public async Task TestUpdate_ReplaceIcon_GeneratesNewKey()
        {
            var typeId = await CreateSecurityType("EquityIcon4");
            var formFile1 = CreateDummyFormFile();
            var formFile2 = CreateDummyFormFile();

            var dto = new SecurityDTO
            {
                Name = "Security Replace Icon",
                Ticker = "SRI2",
                TypeId = typeId,
                CurrencyId = CurrencyConstants.USD,
                ActualPrice = 250m
            };

            var added = await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<ISecurityService>();
                return await service.Add(dto, formFile1);
            });

            var initialKey = added.IconKey;
            Assert.NotNull(initialKey);

            var updated = await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<ISecurityService>();
                return await service.Update(added, formFile2);
            });

            Assert.NotNull(updated.IconKey);
            Assert.NotEqual(initialKey, updated.IconKey);
            Assert.StartsWith(added.Id.ToString(), updated.IconKey);
        }

        private static Microsoft.AspNetCore.Http.IFormFile CreateDummyFormFile()
        {
            var content = System.Text.Encoding.UTF8.GetBytes("dummy security image");
            return new Microsoft.AspNetCore.Http.FormFile(new System.IO.MemoryStream(content), 0, content.Length, "icon", "icon.png");
        }

        private async Task<Guid> CreateSecurityType(string name)
        {
            return await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<ISecurityTypeService>();
                return await service.Add(new SecurityTypeDTO { Name = name });
            });
        }
    }
}
