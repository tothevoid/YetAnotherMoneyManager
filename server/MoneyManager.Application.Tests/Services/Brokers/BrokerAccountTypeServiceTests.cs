using Microsoft.Extensions.DependencyInjection;
using MoneyManager.Application.DTO.Brokers;
using MoneyManager.Application.Interfaces.Brokers;
using MoneyManager.Application.Tests.Fixtures;

namespace MoneyManager.Application.Tests.Services.Brokers
{
    public class BrokerAccountTypeServiceTests : TestBase
    {
        public BrokerAccountTypeServiceTests(ServiceCollectionFixture serviceCollectionFixture) : base(serviceCollectionFixture)
        {
        }

        [Fact]
        public async Task TestAddAndGetAll()
        {
            var typeId = await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<IBrokerAccountTypeService>();
                return await service.Add(new BrokerAccountTypeDTO { Name = "IIS (Tax Free)" });
            });

            Assert.NotEqual(Guid.Empty, typeId);

            var all = await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<IBrokerAccountTypeService>();
                return await service.GetAll();
            });

            Assert.NotNull(all);
            Assert.Contains(all, t => t.Id == typeId && t.Name == "IIS (Tax Free)");
        }

        [Fact]
        public async Task TestUpdate()
        {
            var typeId = await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<IBrokerAccountTypeService>();
                return await service.Add(new BrokerAccountTypeDTO { Name = "Brokerage Initial" });
            });

            await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<IBrokerAccountTypeService>();
                await service.Update(new BrokerAccountTypeDTO { Id = typeId, Name = "Brokerage Updated" });
            });

            var all = await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<IBrokerAccountTypeService>();
                return await service.GetAll();
            });

            var updated = all.FirstOrDefault(t => t.Id == typeId);
            Assert.NotNull(updated);
            Assert.Equal("Brokerage Updated", updated.Name);
        }

        [Fact]
        public async Task TestDelete()
        {
            var typeId = await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<IBrokerAccountTypeService>();
                return await service.Add(new BrokerAccountTypeDTO { Name = "Brokerage Initial" });
            });

            await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<IBrokerAccountTypeService>();
                await service.Delete(typeId);
            });

            var allAfterDelete = await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<IBrokerAccountTypeService>();
                return await service.GetAll();
            });

            Assert.DoesNotContain(allAfterDelete, t => t.Id == typeId);
        }
    }
}
