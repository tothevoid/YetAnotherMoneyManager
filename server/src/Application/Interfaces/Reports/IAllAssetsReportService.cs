using System.Threading.Tasks;
using MoneyManager.Application.DTO.Reports;

namespace MoneyManager.Application.Interfaces.Reports
{
    public interface IAllAssetsReportService
    {
        Task<GeneratedReportDto> CreateReportAsync();
    }
}
