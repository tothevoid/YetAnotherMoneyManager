using Microsoft.Extensions.DependencyInjection;
using MoneyManager.Application.DTO.Brokers;
using MoneyManager.Application.Interfaces.Brokers;
using MoneyManager.Application.Tests.Fixtures;

namespace MoneyManager.Application.Tests.Services.Brokers
{
    public class BrokerServiceTests : TestBase
    {
        public BrokerServiceTests(ServiceCollectionFixture serviceCollectionFixture) : base(serviceCollectionFixture)
        {
        }

        [Fact]
        public async Task TestAddAndGetAll()
        {
            var brokerId = await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<IBrokerService>();
                return await service.Add(new BrokerDTO { Name = "Interactive Brokers" });
            });

            Assert.NotEqual(Guid.Empty, brokerId);

            var all = await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<IBrokerService>();
                return await service.GetAll();
            });

            Assert.NotNull(all);
            Assert.Contains(all, b => b.Id == brokerId && b.Name == "Interactive Brokers");
        }

        [Fact]
        public async Task TestUpdate()
        {
            var actualName = "Interactive Brokers";

            var brokerId = await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<IBrokerService>();
                return await service.Add(new BrokerDTO { Name = "Brokers" });
            });

            await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<IBrokerService>();
                await service.Update(new BrokerDTO { Id = brokerId, Name = actualName });
            });

            var all = await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<IBrokerService>();
                return await service.GetAll();
            });

            var updated = all.FirstOrDefault(b => b.Id == brokerId);
            Assert.NotNull(updated);
            Assert.Equal(actualName, updated.Name);
        }

        [Fact]
        public async Task TestDelete()
        {
            var brokerId = await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<IBrokerService>();
                return await service.Add(new BrokerDTO { Name = "Interactive Brokers" });
            });

            var all = await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<IBrokerService>();
                return await service.GetAll();
            });

            var updated = all.FirstOrDefault(b => b.Id == brokerId);
            Assert.NotNull(updated);

            await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<IBrokerService>();
                await service.Delete(brokerId);
            });

            var allAfterDelete = await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<IBrokerService>();
                return await service.GetAll();
            });

            Assert.DoesNotContain(allAfterDelete, b => b.Id == brokerId);
        }
    }
}
