using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System;
using AutoMapper;
using MoneyManager.Application.DTO.Brokers;
using MoneyManager.Application.Interfaces.Brokers;
using MoneyManager.WebApi.Models.Brokers;

namespace MoneyManager.WebApi.Controllers.Brokers
{
    [Produces("application/json")]
    [Route("[controller]")]
    [ApiController]
    public class BrokerAccountSummaryController : ControllerBase
    {
        private readonly IBrokerAccountSummaryService _brokerAccountSummaryService;

        private readonly IMapper _mapper;
        public BrokerAccountSummaryController(IMapper mapper,
            IBrokerAccountSummaryService brokerAccountSummaryService)
        {
            _mapper = mapper;
            _brokerAccountSummaryService = brokerAccountSummaryService;
        }

        [HttpGet("GetSummary")]
        public async Task<BrokerAccountSummaryModel> GetSummary(Guid brokerAccountId, DateTime from, DateTime to)
        {
            var brokerAccount = await _brokerAccountSummaryService.GetSummary(brokerAccountId, from, to);
            return _mapper.Map<BrokerAccountSummaryModel>(brokerAccount);
        }

        [HttpGet("GetMonthTransfersHistory")]
        public async Task<IEnumerable<BrokerAccountDayTransferModel>> GetMonthTransfersHistory(Guid brokerAccountId, int month, int year)
        {
            var brokerAccount = await _brokerAccountSummaryService.GetMonthTransfersHistory(brokerAccountId, month, year);
            return _mapper.Map<IEnumerable<BrokerAccountDayTransferModel>>(brokerAccount);
        }

        [HttpGet("GetYearTransfersHistory")]
        public async Task<IEnumerable<BrokerAccountMonthTransferModel>> GetYearTransfersHistory(Guid brokerAccountId, int year)
        {
            var brokerAccount = await _brokerAccountSummaryService.GetYearTransfersHistory(brokerAccountId, year);
            return _mapper.Map<IEnumerable<BrokerAccountMonthTransferModel>>(brokerAccount);
        }

        [HttpGet(nameof(GetDailyStats))]
        public async Task<BrokerAccountDailyStatsModel> GetDailyStats(Guid brokerAccountId)
        {
            var brokerAccount = await _brokerAccountSummaryService.GetDailyStats(brokerAccountId);
            return _mapper.Map<BrokerAccountDailyStatsModel>(brokerAccount);
        }
    }
}