#nullable enable
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Audex.Application.Interfaces.DatabaseBackup;

namespace Audex.WebApi.Middlewares
{
    public class DatabaseMaintenanceMiddleware
    {
        private readonly RequestDelegate _next;

        public DatabaseMaintenanceMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, IDatabaseStateService databaseStateService)
        {
            if (databaseStateService.IsRestoring)
            {
                await Results.Problem(
                    statusCode: StatusCodes.Status503ServiceUnavailable,
                    detail: "Database maintenance or restoration is currently in progress. Please retry shortly."
                ).ExecuteAsync(context);
                return;
            }

            await _next(context);
        }
    }
}
