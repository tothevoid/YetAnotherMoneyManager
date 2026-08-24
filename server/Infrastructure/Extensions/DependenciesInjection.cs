using Microsoft.Extensions.DependencyInjection;
using MoneyManager.Infrastructure.Database;
using MoneyManager.Infrastructure.Entities.Scheduler;
using MoneyManager.Infrastructure.Interfaces.Database;
using MoneyManager.Infrastructure.Interfaces.Messages;
using MoneyManager.Infrastructure.Messages;
using TickerQ.DependencyInjection;
using TickerQ.EntityFrameworkCore.Customizer;
using TickerQ.EntityFrameworkCore.DependencyInjection;
using TickerQ.Utilities.Entities;

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

        public static IServiceCollection AddTickerQConfiguration(
            this IServiceCollection services)
        {
            services.AddTickerQ<TimeTickerEntity, ScheduledCronTicker>(opt =>
            {
                opt.AddOperationalStore(ef =>
                {
                    ef.UseApplicationDbContext<ApplicationDbContext>(ConfigurationType.UseModelCustomizer);
                    ef.SetSchema("ticker");
                });
            });

            return services;
        }
    }
}
