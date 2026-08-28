using System.Threading.Tasks;
using Audex.Application.DTO.Reports;

namespace Audex.Application.Interfaces.Reports
{
    public interface IAllAssetsReportService
    {
        Task<GeneratedReportDto> CreateReportAsync();
    }
}
