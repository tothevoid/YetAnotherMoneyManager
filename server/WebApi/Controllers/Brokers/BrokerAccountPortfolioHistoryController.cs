using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MoneyManager.Application.Interfaces.Brokers;
using MoneyManager.Application.Services.Brokers;
using System;
using System.Threading.Tasks;

namespace MoneyManager.WebApi.Controllers.Brokers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class BrokerAccountPortfolioHistoryController : ControllerBase
    {
        private readonly IBrokerAccountPortfolioHistoryService _brokerAccountPortfolioHistoryService;

        public BrokerAccountPortfolioHistoryController(IBrokerAccountPortfolioHistoryService brokerAccountPortfolioHistoryService)
        {
            _brokerAccountPortfolioHistoryService = brokerAccountPortfolioHistoryService;
        }

        [HttpGet(nameof(GetAll))]
        public async Task<BrokerAccountPortfolioHistoryDto> GetAll([FromQuery] DateOnly date)
        {
            return await _brokerAccountPortfolioHistoryService.GetAll(AdjustDate(date));
        }

        [HttpGet(nameof(GetByBrokerAccount))]
        public async Task<BrokerAccountPortfolioHistoryDto> GetByBrokerAccount([FromQuery] DateOnly date, [FromQuery] Guid brokerAccountId)
        {
            return await _brokerAccountPortfolioHistoryService.GetByBrokerAccount(AdjustDate(date), brokerAccountId);
        }

        private DateOnly AdjustDate(DateOnly date)
        {
            var today = DateOnly.FromDateTime(DateTime.Today);
            return date > today ? today : date;
        }
    }
}
