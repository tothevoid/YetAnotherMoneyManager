using Microsoft.Extensions.DependencyInjection;
using MoneyManager.Application.Interfaces.Reports;
using MoneyManager.Application.Tests.Fixtures;

namespace MoneyManager.Application.Tests.Services.Reports
{
    public class AllAssetsReportServiceTests : TestBase
    {
        public AllAssetsReportServiceTests(ServiceProviderFixture serviceProviderFixture) : base(serviceProviderFixture)
        {
        }

        [Fact]
        public async Task TestCreateReport_GeneratesExcelByteArray()
        {
            var reportBytes = await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<IAllAssetsReportService>();
                return await service.CreateReportAsync();
            });

            Assert.NotNull(reportBytes);
            Assert.NotEmpty(reportBytes);
        }
    }
}
