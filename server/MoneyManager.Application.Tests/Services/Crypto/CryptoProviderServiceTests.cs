using Microsoft.Extensions.DependencyInjection;
using MoneyManager.Application.Interfaces.Crypto;
using MoneyManager.Application.Tests.Fixtures;
using MoneyManager.Application.DTO.Crypto;
using MoneyManager.Infrastructure.Entities.Crypto;

namespace MoneyManager.Application.Tests.Services.Crypto
{
    public class CryptoProviderServiceTests : TestBase
    {
        public CryptoProviderServiceTests(ServiceCollectionFixture serviceCollectionFixture) : base(serviceCollectionFixture)
        {
        }

        [Fact]
        public async Task TestAddAndGetAll()
        {
            var providerId = await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<ICryptoProviderService>();
                return await service.Add(new CryptoProviderDto
                {
                    Name = "Binance"
                });
            });

            Assert.NotEqual(Guid.Empty, providerId);

            var all = await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<ICryptoProviderService>();
                return await service.GetAll();
            });

            Assert.NotNull(all);
            Assert.Contains(all, p => p.Id == providerId && p.Name == "Binance");
        }

        [Fact]
        public async Task TestUpdateAndDelete()
        {
            var providerId = await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<ICryptoProviderService>();
                return await service.Add(new CryptoProviderDto
                {
                    Name = "Bybit Initial"
                });
            });

            await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<ICryptoProviderService>();
                await service.Update(new CryptoProviderDto
                {
                    Id = providerId,
                    Name = "Bybit Updated"
                });
            });

            var all = await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<ICryptoProviderService>();
                return await service.GetAll();
            });

            var updated = all.FirstOrDefault(p => p.Id == providerId);
            Assert.NotNull(updated);
            Assert.Equal("Bybit Updated", updated.Name);

            await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<ICryptoProviderService>();
                await service.Delete(providerId);
            });

            var listAfterDelete = await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<ICryptoProviderService>();
                return await service.GetAll();
            });

            Assert.DoesNotContain(listAfterDelete, p => p.Id == providerId);
        }
    }
}
