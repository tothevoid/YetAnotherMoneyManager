using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MoneyManager.Application.Extensions;
using MoneyManager.Application.Interfaces.FileStorage;
using MoneyManager.Infrastructure.Database;
using MoneyManager.Infrastructure.Interfaces.Database;
using MoneyManager.Infrastructure.Interfaces.Messages;
using Testcontainers.PostgreSql;
using Xunit;

namespace MoneyManager.Tests.Shared.Fixtures
{
    public class PostgresDbFixture : IAsyncLifetime
    {
        private readonly PostgreSqlContainer _container = new PostgreSqlBuilder("postgres:17")
            .Build();

        public IServiceProvider ServiceProvider { get; private set; } = null!;

        public string ConnectionString => $"{_container.GetConnectionString()};Pooling=true;MinPoolSize=1;Application Name=MoneyManagerTests;Enlist=false;";

        public async ValueTask InitializeAsync()
        {
            await _container.StartAsync();

            var services = new ServiceCollection();

            var inMemorySettings = new Dictionary<string, string?> {
                {"Auth:Issuer", "MoneyManagerApp"},
                {"Auth:Audience", "MoneyManagerAppUsers"},
                {"Auth:Secret", "SuperSecretKeyForJwtTokenGeneration12345!"}
            };

            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();

            services.AddSingleton<IConfiguration>(configuration);
            services.AddHttpClient();
            services.AddApplicationServices();
            services.AddScoped<IFileStorageService, TestFileStorageService>();
            services.AddScoped<IServerNotifier, TestServerNotifier>();

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(ConnectionString));
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            ServiceProvider = services.BuildServiceProvider();

            using var scope = ServiceProvider.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            await db.Database.MigrateAsync();
        }

        public async ValueTask DisposeAsync()
        {
            await _container.DisposeAsync();
        }

        public ApplicationDbContext CreateDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseNpgsql(ConnectionString)
                .Options;

            return new ApplicationDbContext(options);
        }
    }
}
