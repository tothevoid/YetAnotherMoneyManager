using System;
using System.Data.Common;
using System.Net.Sockets;
using System.Threading;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Audex.Infrastructure.Database;

namespace Audex.Infrastructure.Extensions
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
                catch (Exception ex) when (IsTransientDatabaseException(ex) && retryCount < maxRetries - 1)
                {
                    retryCount++;
                    logger.LogWarning("Database is not ready yet ({Reason}). Waiting {Delay}ms before retry {RetryCount}/{MaxRetries}...", ex.Message, delayMilliseconds, retryCount, maxRetries);
                    Thread.Sleep(delayMilliseconds);
                }
            }

            return host;
        }

        private static bool IsTransientDatabaseException(Exception ex)
        {
            return ex is DbException ||
                   ex is SocketException ||
                   ex.InnerException is DbException ||
                   ex.InnerException is SocketException;
        }
    }
}
