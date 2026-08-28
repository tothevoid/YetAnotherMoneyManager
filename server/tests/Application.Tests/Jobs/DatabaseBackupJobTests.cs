using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using MoneyManager.Application.DTO.Scheduler;
using MoneyManager.Application.Enums.Scheduler;
using MoneyManager.Application.Jobs;
using MoneyManager.Application.Tests.Fixtures;
using Xunit;

namespace MoneyManager.Application.Tests.Jobs
{
    [Trait("Category", "S3")]
    public class DatabaseBackupJobTests : TestBase
    {
        public DatabaseBackupJobTests(ServiceProviderFixture serviceProviderFixture) : base(serviceProviderFixture)
        {
        }

        [Fact]
        public async Task TestDatabaseBackupJob_ExecutesSuccessfully()
        {
            await ExecuteScopeAsync(async sp =>
            {
                var job = sp.GetRequiredService<DatabaseBackupJob>();

                await job.ExecuteAsync(triggerSource: ScheduledTaskTriggerSource.Manual);
            });
        }
    }
}
