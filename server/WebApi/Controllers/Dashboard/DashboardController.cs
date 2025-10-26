using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System;
using AutoMapper;
using MoneyManager.Application.DTO.Securities;
using MoneyManager.WebApi.Models.Securities;
using MoneyManager.Application.Interfaces.Securities;
using MoneyManager.Application.Interfaces.Currencies;
using MoneyManager.Application.Services.Dashboard;
using MoneyManager.WebApi.Models.Dashboard;

namespace MoneyManager.WebApi.Controllers.Dashboard
{
    [Produces("application/json")]
    [Route("[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IDashboardService _dashboardService;

        public DashboardController(IMapper mapper, IDashboardService dashboardService)
        {
            _mapper = mapper;
            _dashboardService = dashboardService;
        }

        public async Task<GlobalDashboardModel> Get()
        {
            var dasboard = await _dashboardService.GetDashboard();
            return _mapper.Map<GlobalDashboardModel>(dasboard);
        }
    }
}