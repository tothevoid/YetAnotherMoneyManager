using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MoneyManager.Infrastructure.Database;
using MoneyManager.Infrastructure.Extensions;
using MoneyManager.Infrastructure.Interfaces.Database;
using MoneyManager.Infrastructure.Interfaces.Messages;
using Xunit;

namespace MoneyManager.Infrastructure.Tests.Extensions
{
    public class DependenciesInjectionTests
    {
        [Fact]
        public void AddInfrastructureServices_RegistersRequiredServices()
        {
            var services = new ServiceCollection();

            services.AddLogging();
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql("Host=localhost;Database=test;Username=postgres;Password=postgres"));
            services.AddSignalR();
            services.AddInfrastructureServices();

            var serviceProvider = services.BuildServiceProvider();

            using var scope = serviceProvider.CreateScope();
            var unitOfWork = scope.ServiceProvider.GetService<IUnitOfWork>();
            var serverNotifier = scope.ServiceProvider.GetService<IServerNotifier>();

            Assert.NotNull(unitOfWork);
            Assert.IsType<UnitOfWork>(unitOfWork);
            Assert.NotNull(serverNotifier);
        }
    }
}
