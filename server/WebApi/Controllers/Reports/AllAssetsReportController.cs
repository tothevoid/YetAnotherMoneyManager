using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using MoneyManager.Application.Interfaces.Reports;

namespace MoneyManager.WebApi.Controllers.Reports
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class AllAssetsReportController : ControllerBase
    {
        private readonly IAllAssetsReportService _reportService;
        public AllAssetsReportController(IAllAssetsReportService reportService)
        {
            _reportService = reportService;
        }

        [HttpGet("xlsx")]
        public async Task<IActionResult> GetAllAssetsReportXlsx()
        {
            var fileBytes = await _reportService.CreateReport();
            return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "all-assets-report.xlsx");
        }
    }
}
