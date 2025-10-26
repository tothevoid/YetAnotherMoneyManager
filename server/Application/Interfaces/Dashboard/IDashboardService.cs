using MoneyManager.Application.DTO.Dashboard;
using System.Threading.Tasks;

namespace MoneyManager.Application.Interfaces.Currencies
{
    public interface IDashboardService
    {
        public Task<GlobalDashboardDto> GetDashboard();
    }
}