using Microsoft.Extensions.DependencyInjection;
using MoneyManager.Application.DTO.Securities;
using MoneyManager.Application.Interfaces.Securities;
using MoneyManager.Application.Tests.Fixtures;
using MoneyManager.Infrastructure.Constants;

namespace MoneyManager.Application.Tests.Services.Securities
{
    public class DividendServiceTests : TestBase
    {
        public DividendServiceTests(ServiceCollectionFixture serviceCollectionFixture) : base(serviceCollectionFixture)
        {
        }

        [Fact]
        public async Task TestAddAndGetAll()
        {
            var securityId = await SetupSecurity();
            var today = DateOnly.FromDateTime(DateTime.Now);

            var divId = await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<IDividendService>();
                return await service.Add(new DividendDto
                {
                    SecurityId = securityId,
                    Amount = 2.5m,
                    DeclarationDate = today.AddMonths(-1),
                    SnapshotDate = today
                });
            });

            Assert.NotEqual(Guid.Empty, divId);

            var all = await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<IDividendService>();
                return await service.GetAll(securityId, 1, 10);
            });

            Assert.NotNull(all);
            Assert.Contains(all, d => d.Id == divId && d.Amount == 2.5m);
        }

        [Fact]
        public async Task TestUpdateAndDelete()
        {
            var securityId = await SetupSecurity();
            var today = DateOnly.FromDateTime(DateTime.Now);

            var divId = await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<IDividendService>();
                return await service.Add(new DividendDto
                {
                    SecurityId = securityId,
                    Amount = 1.0m,
                    DeclarationDate = today,
                    SnapshotDate = today
                });
            });

            await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<IDividendService>();
                await service.Update(new DividendDto
                {
                    Id = divId,
                    SecurityId = securityId,
                    Amount = 3.0m,
                    DeclarationDate = today,
                    SnapshotDate = today
                });
            });

            var all = await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<IDividendService>();
                return await service.GetAll(securityId, 1, 10);
            });

            var updated = all.FirstOrDefault(d => d.Id == divId);
            Assert.NotNull(updated);
            Assert.Equal(3.0m, updated.Amount);

            await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<IDividendService>();
                await service.Delete(divId);
            });

            var listAfterDelete = await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<IDividendService>();
                return await service.GetAll(securityId, 1, 10);
            });

            Assert.DoesNotContain(listAfterDelete, d => d.Id == divId);
        }

        private async Task<Guid> SetupSecurity()
        {
            var securityTypeId = await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<ISecurityTypeService>();
                return await service.Add(new SecurityTypeDTO { Name = "Dividend Stock Type" });
            });

            return await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<ISecurityService>();
                var sec = await service.Add(new SecurityDTO
                {
                    Name = "Dividend Payer Inc",
                    Ticker = "DIV",
                    TypeId = securityTypeId,
                    CurrencyId = CurrencyConstants.USD,
                    ActualPrice = 100m
                }, null);
                return sec.Id;
            });
        }
    }
}
