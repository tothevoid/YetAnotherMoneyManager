using AutoMapper;
using ClosedXML.Parser;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MoneyManager.Application.Extensions;
using MoneyManager.Infrastructure.Database;
using MoneyManager.Infrastructure.Interfaces.Database;
using Testcontainers.PostgreSql;

namespace MoneyManager.Application.Tests.Fixtures
{
    public class ServiceCollectionFixture : IAsyncLifetime
    {
        private readonly PostgreSqlContainer _container = new PostgreSqlBuilder("postgres:17")
            .Build();

        public IServiceProvider ServiceProvider { get; private set; }

        public string ConnectionString => _container.GetConnectionString();

        public async Task InitializeAsync()
        {
            await _container.StartAsync();

            var services = new ServiceCollection();
            services.AddApplicationServices();

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseNpgsql(ConnectionString)
                .Options;


            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(_container.GetConnectionString()));
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            var mapper = new MapperConfiguration(cfg =>
            {
                cfg.AddApplicationProfile();
            }).CreateMapper();

            services.AddSingleton(mapper);

            ServiceProvider = services.BuildServiceProvider();

             using var scope = ServiceProvider.CreateScope();
             var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
             await db.Database.MigrateAsync();
        }

        public async Task DisposeAsync()
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
