using System;
using System.Threading;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MoneyManager.Infrastructure.Database;

namespace MoneyManager.Infrastructure.Extensions
{
    public static class DatabaseMigrationExtensions
    {
        public static IHost MigrateDatabase(this IHost host, int maxRetries = 10, int delayMilliseconds = 3000)
        {
            using var scope = host.Services.CreateScope();
            var services = scope.ServiceProvider;
            var logger = services.GetRequiredService<ILogger<ApplicationDbContext>>();
            var retryCount = 0;

            while (retryCount < maxRetries)
            {
                try
                {
                    var db = services.GetRequiredService<ApplicationDbContext>();
                    db.Database.Migrate();
                    logger.LogInformation("Database migration completed successfully.");
                    break;
                }
                catch (Exception ex) when (retryCount < maxRetries - 1)
                {
                    retryCount++;
                    logger.LogWarning(ex, "Database is not ready yet. Waiting {Delay}ms before retry {RetryCount}/{MaxRetries}...", delayMilliseconds, retryCount, maxRetries);
                    Thread.Sleep(delayMilliseconds);
                }
            }

            return host;
        }
    }
}
