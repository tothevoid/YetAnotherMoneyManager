using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using InfrastructureManager.Application.Interfaces;
using InfrastructureManager.Application.Services;

namespace InfrastructureManager.Application.Extensions
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureManagerApplication(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddTransient<IPostgresBackupService, PostgresBackupService>();
            return services;
        }
    }
}
