#nullable enable
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MoneyManager.Application.Interfaces.DatabaseBackup
{
    public interface IDatabaseStateService
    {
        bool IsRestoring { get; }
        Task<IDisposable> BeginRestoreScopeAsync(CancellationToken cancellationToken = default);
    }
}
