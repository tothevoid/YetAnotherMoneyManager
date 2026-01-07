using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System;
using AutoMapper;
using MoneyManager.Application.DTO.Brokers;
using MoneyManager.Application.Interfaces.Brokers;
using MoneyManager.WebApi.Models.Brokers;
using Microsoft.AspNetCore.Authorization;

namespace MoneyManager.WebApi.Controllers.Brokers
{
    [Produces("application/json")]
    [Route("[controller]")]
    [ApiController]
    [Authorize]
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

        [HttpGet(nameof(GetSummaryByBrokerAccount))]
        public async Task<BrokerAccountSummaryModel> GetSummaryByBrokerAccount(Guid brokerAccountId, DateTime from, DateTime to)
        {
            var brokerAccount = await _brokerAccountSummaryService.GetSummaryByBrokerAccount(brokerAccountId);
            return _mapper.Map<BrokerAccountSummaryModel>(brokerAccount);
        }

        [HttpGet(nameof(GetSummary))]
        public async Task<BrokerAccountSummaryModel> GetSummary(DateTime from, DateTime to)
        {
            var brokerAccount = await _brokerAccountSummaryService.GetSummary();
            return _mapper.Map<BrokerAccountSummaryModel>(brokerAccount);
        }

        [HttpGet(nameof(GetMonthTransfersHistory))]
        public async Task<IEnumerable<BrokerAccountDayTransferModel>> GetMonthTransfersHistory(int month, int year)
        {
            var brokerAccount = await _brokerAccountSummaryService.GetMonthTransfersHistory(month, year);
            return _mapper.Map<IEnumerable<BrokerAccountDayTransferModel>>(brokerAccount);
        }

        [HttpGet(nameof(GetMonthTransfersHistoryByBrokerAccount))]
        public async Task<IEnumerable<BrokerAccountDayTransferModel>> GetMonthTransfersHistoryByBrokerAccount(Guid brokerAccountId, int month, int year)
        {
            var brokerAccount = await _brokerAccountSummaryService.GetMonthTransfersHistoryByBrokerAccount(brokerAccountId, month, year);
            return _mapper.Map<IEnumerable<BrokerAccountDayTransferModel>>(brokerAccount);
        }

        [HttpGet(nameof(GetYearTransfersHistory))]
        public async Task<IEnumerable<BrokerAccountMonthTransferModel>> GetYearTransfersHistory(int year)
        {
            var brokerAccount = await _brokerAccountSummaryService.GetYearTransfersHistory(year);
            return _mapper.Map<IEnumerable<BrokerAccountMonthTransferModel>>(brokerAccount);
        }

        [HttpGet(nameof(GetYearTransfersHistoryByBrokerAccount))]
        public async Task<IEnumerable<BrokerAccountMonthTransferModel>> GetYearTransfersHistoryByBrokerAccount(Guid brokerAccountId, int year)
        {
            var brokerAccount = await _brokerAccountSummaryService.GetYearTransfersHistoryByBrokerAccount(brokerAccountId, year);
            return _mapper.Map<IEnumerable<BrokerAccountMonthTransferModel>>(brokerAccount);
        }

        [HttpGet(nameof(GetDailyStatsByBrokerAccount))]
        public async Task<BrokerAccountDailyStatsModel> GetDailyStatsByBrokerAccount([FromQuery] Guid brokerAccountId)
        {
            var brokerAccount = await _brokerAccountSummaryService.GetDailyStatsByBrokerAccount(brokerAccountId);
            return _mapper.Map<BrokerAccountDailyStatsModel>(brokerAccount);
        }

        [HttpGet(nameof(GetDailyStats))]
        public async Task<BrokerAccountDailyStatsModel> GetDailyStats()
        {
            var brokerAccount = await _brokerAccountSummaryService.GetDailyStats();
            return _mapper.Map<BrokerAccountDailyStatsModel>(brokerAccount);
        }
        
    }
}