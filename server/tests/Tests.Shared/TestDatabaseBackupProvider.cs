using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Audex.Infrastructure.Interfaces.DatabaseBackup;

namespace Audex.Tests.Shared
{
    public class TestDatabaseBackupProvider : IDatabaseBackupProvider
    {
        public Task<byte[]> ExportDatabaseDumpAsync(CancellationToken cancellationToken = default)
        {
            var fakeDump = Encoding.UTF8.GetBytes("CREATE TABLE test(); INSERT INTO test VALUES (1);");
            return Task.FromResult(fakeDump);
        }

        public Task ImportDatabaseDumpAsync(byte[] dumpData, CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }
    }
}
