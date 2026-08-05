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
