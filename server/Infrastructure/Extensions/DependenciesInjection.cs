using Microsoft.Extensions.DependencyInjection;
using MoneyManager.Infrastructure.Database;
using MoneyManager.Infrastructure.Interfaces.Database;
using MoneyManager.Infrastructure.Interfaces.Messages;
using MoneyManager.Infrastructure.Messages;

namespace MoneyManager.Infrastructure.Extensions
{
    public static class DependenciesInjection
    {
        public static IServiceCollection AddInfrastructureServices(
            this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IServerNotifier, ServerNotifier>();

            return services;
        }
    }
}
