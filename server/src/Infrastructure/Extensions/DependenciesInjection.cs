using Microsoft.Extensions.DependencyInjection;
using Audex.Infrastructure.Database;
using Audex.Infrastructure.Entities.Scheduler;
using Audex.Infrastructure.Interfaces.Database;
using Audex.Infrastructure.Interfaces.Messages;
using Audex.Infrastructure.Messages;
using TickerQ.DependencyInjection;
using TickerQ.EntityFrameworkCore.Customizer;
using TickerQ.EntityFrameworkCore.DependencyInjection;
using TickerQ.Utilities.Entities;

namespace Audex.Infrastructure.Extensions
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
