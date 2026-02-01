using System.Threading.Tasks;

namespace MoneyManager.Application.Interfaces.Reports
{
    public interface IAllAssetsReportService
    {
        Task<byte[]> CreateReport();
    }
}
