using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Minio;
using MoneyManager.Application.Extensions;
using MoneyManager.Application.Interfaces.FileStorage;
using MoneyManager.Application.Services.FileStorage;
using MoneyManager.Infrastructure.Database;
using MoneyManager.Infrastructure.Interfaces.Database;
using MoneyManager.Infrastructure.Extensions;
using MoneyManager.Infrastructure.Interfaces.Messages;
using Testcontainers.Minio;
using Testcontainers.PostgreSql;
using Xunit;

namespace MoneyManager.Tests.Shared.Fixtures
{
    public class ServiceProviderFixture : IAsyncLifetime
    {
        private readonly PostgreSqlContainer _postgresContainer = new PostgreSqlBuilder("postgres:17")
            .Build();

        private readonly MinioContainer _minioContainer = new MinioBuilder("minio/minio")
            .Build();

        public IServiceProvider ServiceProvider { get; private set; } = null!;

        public string ConnectionString => $"{_postgresContainer.GetConnectionString()};Pooling=true;MinPoolSize=1;Application Name=MoneyManagerTests;Enlist=false;";

        public string MinioEndpoint => _minioContainer.GetConnectionString().Replace("http://", "").Replace("https://", "");
        public string MinioAccessKey => _minioContainer.GetAccessKey();
        public string MinioSecretKey => _minioContainer.GetSecretKey();

        public async ValueTask InitializeAsync()
        {
            await Task.WhenAll(_postgresContainer.StartAsync(), _minioContainer.StartAsync());

            var services = new ServiceCollection();

            var inMemorySettings = new Dictionary<string, string?> {
                {"Auth:Issuer", "MoneyManagerApp"},
                {"Auth:Audience", "MoneyManagerAppUsers"},
                {"Auth:Secret", "SuperSecretKeyForJwtTokenGeneration12345!"},
                {"FileStorage:Endpoint", MinioEndpoint},
                {"FileStorage:UseSsl", "false"},
                {"FileStorage:User", MinioAccessKey},
                {"FileStorage:Password", MinioSecretKey},
                {"DB:ConnectionString", ConnectionString}
            };

            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();

            services.AddSingleton<IConfiguration>(configuration);
            services.AddHttpClient();
            services.AddApplicationServices();
            services.AddInfrastructureServices();

            services.AddSingleton<MoneyManager.Infrastructure.Interfaces.DatabaseBackup.IDatabaseBackupProvider, TestDatabaseBackupProvider>();

            services.AddMinio(configureClient => configureClient
                .WithEndpoint(MinioEndpoint)
                .WithSSL(false)
                .WithCredentials(MinioAccessKey, MinioSecretKey)
                .Build());

            services.AddScoped<IServerNotifier, TestServerNotifier>();

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(ConnectionString));

            services.AddTickerQConfiguration();

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            ServiceProvider = services.BuildServiceProvider();

            using var scope = ServiceProvider.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            await db.Database.MigrateAsync();
        }

        public async ValueTask DisposeAsync()
        {
            await Task.WhenAll(_postgresContainer.DisposeAsync().AsTask(), _minioContainer.DisposeAsync().AsTask());
        }

        public ApplicationDbContext CreateDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseNpgsql(ConnectionString)
                .Options;

            return new ApplicationDbContext(options);
        }

        public IMinioClient CreateMinioClient()
        {
            return new MinioClient()
                .WithEndpoint(MinioEndpoint)
                .WithSSL(false)
                .WithCredentials(MinioAccessKey, MinioSecretKey)
                .Build();
        }

        public IFileStorageService CreateFileStorageService()
        {
            return new FileStorageService(CreateMinioClient());
        }
    }
}