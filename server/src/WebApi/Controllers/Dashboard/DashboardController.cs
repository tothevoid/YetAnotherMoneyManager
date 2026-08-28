using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MoneyManager.Application.Interfaces.Dashboard;
using MoneyManager.WebApi.Mappings;
using MoneyManager.WebApi.Models.Dashboard;
using Microsoft.AspNetCore.Authorization;

namespace MoneyManager.WebApi.Controllers.Dashboard
{
    [Produces("application/json")]
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class DashboardController : ControllerBase
    {
        private readonly WebApiMapper _mapper;
        private readonly IDashboardService _dashboardService;

        public DashboardController(WebApiMapper mapper, IDashboardService dashboardService)
        {
            _mapper = mapper;
            _dashboardService = dashboardService;
        }

        public async Task<GlobalDashboardModel> Get()
        {
            var dasboard = await _dashboardService.GetDashboardAsync();
            return _mapper.Map(dasboard);
        }
    }
}