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
            var report = await ExecuteScopeAsync(async sp =>
            {
                var service = sp.GetRequiredService<IAllAssetsReportService>();
                return await service.CreateReportAsync();
            });

            Assert.NotNull(report);
            Assert.NotNull(report.Data);
            Assert.NotEmpty(report.Data);
            Assert.Equal("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", report.ContentType);
            Assert.EndsWith(".xlsx", report.FileName);
        }
    }
}
