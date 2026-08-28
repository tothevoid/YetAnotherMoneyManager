using Audex.Application.DTO.Dashboard;
using System.Threading.Tasks;

namespace Audex.Application.Interfaces.Dashboard
{
    public interface IDashboardService
    {
        public Task<GlobalDashboardDto> GetDashboardAsync();
    }
}