using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using MoneyManager.Application.Interfaces.Scheduler;
using MoneyManager.Application.Tests.Fixtures;
using Xunit;

namespace MoneyManager.Application.Tests.Services.Scheduler
{
    [Trait("Category", "S3")]
    public class SchedulerAttachmentServiceTests : TestBase
    {
        public SchedulerAttachmentServiceTests(ServiceProviderFixture serviceProviderFixture) : base(serviceProviderFixture)
        {
        }

        [Fact]
        public async Task GetAttachmentFileStreamAsync_NonExistentAttachment_ReturnsNull()
        {
            await ExecuteScopeAsync(async sp =>
            {
                var attachmentService = sp.GetRequiredService<ISchedulerAttachmentService>();

                var result = await attachmentService.GetAttachmentFileStreamAsync(Guid.NewGuid());

                Assert.Null(result);
            });
        }
    }
}
