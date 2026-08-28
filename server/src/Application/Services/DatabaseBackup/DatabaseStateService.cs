#nullable enable
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Audex.Application.Interfaces.DatabaseBackup;

namespace Audex.Application.Services.DatabaseBackup
{
    public class DatabaseStateService : IDatabaseStateService
    {
        private readonly SemaphoreSlim _lock = new(1, 1);
        private readonly ILogger<DatabaseStateService> _logger;
        private volatile bool _isRestoring = false;

        public bool IsRestoring => _isRestoring;

        public DatabaseStateService(ILogger<DatabaseStateService>? logger = null)
        {
            _logger = logger ?? NullLogger<DatabaseStateService>.Instance;
        }

        public async Task<IDisposable> BeginRestoreScopeAsync(CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Waiting to acquire database maintenance lock for restore...");
            await _lock.WaitAsync(cancellationToken);
            _isRestoring = true;
            _logger.LogInformation("Database maintenance lock acquired. Restore scope started.");
            return new RestoreScope(this);
        }

        private void EndRestoreScope()
        {
            _isRestoring = false;
            try
            {
                _lock.Release();
                _logger.LogInformation("Database maintenance lock released. Restore scope ended.");
            }
            catch (SemaphoreFullException ex)
            {
                _logger.LogWarning(ex, "Database maintenance lock was already released when ending restore scope.");
            }
        }

        private sealed class RestoreScope : IDisposable
        {
            private DatabaseStateService? _service;

            public RestoreScope(DatabaseStateService service)
            {
                _service = service;
            }

            public void Dispose()
            {
                var service = Interlocked.Exchange(ref _service, null);
                service?.EndRestoreScope();
            }
        }
    }
}
