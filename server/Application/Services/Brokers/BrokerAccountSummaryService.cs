using AutoMapper;
using MoneyManager.Application.DTO.Brokers;
using MoneyManager.Application.Interfaces.Brokers;
using MoneyManager.Application.Interfaces.Integrations.Stock;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoneyManager.Application.Services.Brokers
{
    public class BrokerAccountSummaryService: IBrokerAccountSummaryService
    {
        private readonly IBrokerAccountSecurityService _brokerAccountSecurityService;
        private readonly IBrokerAccountFundsTransferService _fundsTransferService;
        private readonly IBrokerAccountService _brokerAccountService;
        private readonly IDividendPaymentService _dividendPaymentService;
        private readonly IStockConnector _stockConnector;

        public BrokerAccountSummaryService(
            IBrokerAccountSecurityService brokerAccountSecuritySecurityService,
            IBrokerAccountFundsTransferService fundsTransferService,
            IBrokerAccountService brokerAccountService,
            IDividendPaymentService dividendPaymentService,
            IStockConnector stockConnector)
        {
            _brokerAccountSecurityService = brokerAccountSecuritySecurityService;
            _fundsTransferService = fundsTransferService;
            _stockConnector = stockConnector;
            _brokerAccountService = brokerAccountService;
            _dividendPaymentService = dividendPaymentService;
        }

        public async Task<BrokerAccountSummaryDto> GetSummary(Guid brokerAccountId, DateTime from, DateTime to)
        {
            //DailyStats = (await GetDailyStats(brokerAccountId, from, to)).ToList(),

            return new BrokerAccountSummaryDto()
            {
                TransferStats = await GetTransfersStats(brokerAccountId),
                BrokerAccountStats = await GetBrokerAccountStats(brokerAccountId, from, to)
            };
        }

        public async Task<IEnumerable<BrokerAccountDayTransferDto>> GetMonthTransfersHistory(Guid brokerAccountId, int month, int year)
        {
            //TODO: Add db month and year filter

            var transfers = await _fundsTransferService.GetAllAsync(brokerAccountId);

            return transfers.Where(transfer => transfer.Date.Year == year && transfer.Date.Month == month)
                .Select(transfer => new BrokerAccountDayTransferDto()
                    { DayIndex = transfer.Date.Day, Income = transfer.Income });
        }

        public async Task<IEnumerable<BrokerAccountMonthTransferDto>> GetYearTransfersHistory(Guid brokerAccountId, int year)
        {
            //TODO: Add db year filter

            var transfers = await _fundsTransferService.GetAllAsync(brokerAccountId);

            return transfers.Where(transfer => transfer.Date.Year == year)
                .Select(transfer => new BrokerAccountMonthTransferDto()
                    { MonthIndex = transfer.Date.Month, Income = transfer.Income });
        }

        private async Task<BrokerAccountTransfersStatsDto> GetTransfersStats(Guid brokerAccountId)
        {
            var transfers = (await _fundsTransferService.GetAllAsync(brokerAccountId)).ToList();

            //bool TransfersFilter(BrokerAccountFundsTransferDto transfer) =>
            //    transfer.Date >= from && transfer.Date <= to;

            var deposits = transfers
                //.Where(TransfersFilter)
                .Where(transfer => transfer.Income);

            var withdrawals = transfers
                //.Where(TransfersFilter)
                .Where(transfer => !transfer.Income);

            var totalDeposit = deposits.Sum(transfer => transfer.Amount);
            var totalWithdraw = withdrawals.Sum(transfer => transfer.Amount);


            return new BrokerAccountTransfersStatsDto()
            {
                TotalDeposited = totalDeposit,
                TotalWithdrawn = totalWithdraw
            };
        }

        private async Task<BrokerAccountStatsDto> GetBrokerAccountStats(Guid brokerAccountId, DateTime from, DateTime to)
        {
            var portfolioValues = await _brokerAccountService.GetPortfolioValues(brokerAccountId);
            var totalDividends = await _dividendPaymentService.GetEarningsByBrokerAccount(brokerAccountId);

            return new BrokerAccountStatsDto()
            {
                CurrentValue = portfolioValues.CurrentValue,
                InvestedValue = portfolioValues.InitialValue,
                TotalDividendsValue = totalDividends
            };
        }

        //private async Task<IEnumerable<DailyStatDto>> GetDailyStats(Guid brokerAccountId, DateTime from, DateTime to)
        //{
        //    var securities = (await _brokerAccountSecurityService
        //            .GetByBrokerAccount(brokerAccountId))
        //        .ToList();


        //    var tickers = securities.Select(security => security.Security.Ticker);
        //    var tickersHistory = await _stockConnector
        //        .GetValuesByTickersInRange(tickers, DateOnly.FromDateTime(from), DateOnly.FromDateTime(to));

        //    var historyByTikcers = tickersHistory.MarketData
        //        .GroupBy(history => history.Ticker);


        //    var dailyStats = new List<DailyStatDto>();

        //    foreach (var security in securities)
        //    {
        //        var hasHistory = tickersHistory.ContainsKey(security.Security.Ticker);

        //        if (!hasHistory)
        //        {
        //            continue;
        //        }

        //        var history = tickersHistory[security.Security.Ticker]
        //            .OrderBy(value => value.Date)
        //            .ToList();

        //        if (!history.Any())
        //        {
        //            continue;
        //        }

        //        dailyStats.Add(new DailyStatDto()
        //        {
        //            Security = security.Security,
        //            MinPrice = history.Min(value => value.Value),
        //            MaxPrice = history.Max(value => value.Value),
        //            FirstPrice = history.First().Value,
        //            LastPrice = history.Last().Value
        //        });
        //    }

        //    return dailyStats;
        //}
    }
}
