using System.Threading.Tasks;
using MoneyManager.Application.Services.DatabaseBackup;
using Xunit;

namespace MoneyManager.Application.Tests.Services.DatabaseBackup
{
    public class DatabaseStateServiceTests
    {
        [Fact]
        public void InitialState_ShouldNotBeRestoring()
        {
            var service = new DatabaseStateService();
            Assert.False(service.IsRestoring);
        }

        [Fact]
        public async Task BeginRestoreScopeAsync_ShouldSetIsRestoringTrue_AndResetOnDispose()
        {
            var service = new DatabaseStateService();

            using (var scope = await service.BeginRestoreScopeAsync())
            {
                Assert.True(service.IsRestoring);
            }

            Assert.False(service.IsRestoring);
        }

        [Fact]
        public async Task MultipleSequentialScopes_ShouldWorkCorrectly()
        {
            var service = new DatabaseStateService();

            using (await service.BeginRestoreScopeAsync())
            {
                Assert.True(service.IsRestoring);
            }
            Assert.False(service.IsRestoring);

            using (await service.BeginRestoreScopeAsync())
            {
                Assert.True(service.IsRestoring);
            }
            Assert.False(service.IsRestoring);
        }
    }
}
