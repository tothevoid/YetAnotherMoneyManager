using MoneyManager.Application.DTO.Dashboard;
using System.Threading.Tasks;

namespace MoneyManager.Application.Interfaces.Dashboard
{
    public interface IDashboardService
    {
        public Task<GlobalDashboardDto> GetDashboardAsync();
    }
}