using Microsoft.Extensions.DependencyInjection;
using Audex.Application.Interfaces.Crypto;
using Audex.Application.Tests.Fixtures;
using Audex.Application.DTO.Crypto;
using Audex.Infrastructure.Entities.Crypto;

namespace Audex.Application.Tests.Services.Crypto
{
    public class CryptoProviderServiceTests : TestBase
    {
        public CryptoProviderServiceTests(ServiceProviderFixture serviceProviderFixture) : base(serviceProviderFixture)
        {
        }

        [Fact]
        public async Task TestAddAndGetAll()
        {
            var providerId = await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<ICryptoProviderService>();
                return await service.AddAsync(new CryptoProviderDto
                {
                    Name = "Binance"
                });
            });

            Assert.NotEqual(Guid.Empty, providerId);

            var all = await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<ICryptoProviderService>();
                return await service.GetAllAsync();
            });

            Assert.NotNull(all);
            Assert.Contains(all, p => p.Id == providerId && p.Name == "Binance");
        }

        [Fact]
        public async Task TestUpdate()
        {
            var providerId = await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<ICryptoProviderService>();
                return await service.AddAsync(new CryptoProviderDto
                {
                    Name = "Bybit Initial"
                });
            });

            await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<ICryptoProviderService>();
                await service.UpdateAsync(new CryptoProviderDto
                {
                    Id = providerId,
                    Name = "Bybit Updated"
                });
            });

            var all = await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<ICryptoProviderService>();
                return await service.GetAllAsync();
            });

            var updated = all.FirstOrDefault(p => p.Id == providerId);
            Assert.NotNull(updated);
            Assert.Equal("Bybit Updated", updated.Name);
        }

        [Fact]
        public async Task TestDelete()
        {
            var providerId = await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<ICryptoProviderService>();
                return await service.AddAsync(new CryptoProviderDto
                {
                    Name = "Bybit Initial"
                });
            });

            await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<ICryptoProviderService>();
                await service.DeleteAsync(providerId);
            });

            var listAfterDelete = await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<ICryptoProviderService>();
                return await service.GetAllAsync();
            });

            Assert.DoesNotContain(listAfterDelete, p => p.Id == providerId);
        }
    }
}
