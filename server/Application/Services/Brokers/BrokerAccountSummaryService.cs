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

        public async Task<BrokerAccountDailyStatsDto> GetDailyStats(Guid brokerAccountId)
        {
            var securities = await _brokerAccountSecurityService
                .GetByBrokerAccount(brokerAccountId);

            var tickerMapping = securities
                .ToDictionary(key => key.Security.Ticker, value => value);

            var tickers = securities.Select(security => security.Security.Ticker);
            var marketValues = (await _stockConnector
                .GetExtendedValuesByTickers(tickers));

            var brokerAccount = await _brokerAccountService.GetById(brokerAccountId);

            var handledTickers = new HashSet<string>();

            var securityStats = new List<BrokerAccountDailySecurityStatsDto>();

            var startPortfolioValue = brokerAccount.MainCurrencyAmount;
            var currentPortfolioValue = brokerAccount.MainCurrencyAmount;

            foreach (var marketValue in marketValues)
            {
                if (handledTickers.Contains(marketValue.Ticker))
                {
                    continue;
                }

                var currentPrice = marketValue.GetLastValue();
                var startPrice = marketValue.PrevPrice ?? marketValue.Open ?? 0;

                var brokerAccountSecurity = tickerMapping[marketValue.Ticker];

                startPortfolioValue += startPrice * brokerAccountSecurity.Quantity;
                currentPortfolioValue += currentPrice * brokerAccountSecurity.Quantity;

                securityStats.Add(new BrokerAccountDailySecurityStatsDto()
                {
                    CurrentPrice = currentPrice,
                    Security = brokerAccountSecurity.Security,
                    StartPrice = marketValue.Open ?? 0,
                    MinPrice = marketValue.Low,
                    MaxPrice = marketValue.High,
                    PreviousDayClosePrice = marketValue.PrevPrice ?? 0,
                    Quantity = brokerAccountSecurity.Quantity
                });

                handledTickers.Add(marketValue.Ticker);
            }

            return new BrokerAccountDailyStatsDto()
            {
                FetchDate = DateTime.UtcNow,
                BrokerAccountDailySecurityStats = securityStats.OrderBy(stat => stat.CurrentPrice - stat.PreviousDayClosePrice),
                CurrentPortfolioValue = currentPortfolioValue,
                StartPortfolioValue = startPortfolioValue
            };
        }

        public async Task<IEnumerable<BrokerAccountDayTransferDto>> GetMonthTransfersHistory(Guid brokerAccountId, int month, int year)
        {
            //TODO: Add db month and year filter
            //TODO: fix GetMonthTransfersHistory & GetYearTransfersHistory code duplication

            var transfers = await _fundsTransferService.GetAll(brokerAccountId);

            var filteredTransfers = transfers
                .Where(transfer => transfer.Date.Year == year && transfer.Date.Month == month);

            var maxDay = new DateOnly(year, month, 1).AddMonths(1).AddDays(-1).Day;

            var distributions = Enumerable.Range(1, maxDay).ToDictionary(k => k, v => new TransfersHistory());

            foreach (var transfer in filteredTransfers)
            {
                var distribution = distributions[transfer.Date.Day];

                if (transfer.Income)
                {
                    distribution.TotalDeposited += transfer.Amount;
                }
                else
                {
                    distribution.TotalWithdrawn += transfer.Amount;
                }
            }

            return distributions.Select(distribution => new BrokerAccountDayTransferDto()
            {
                DayIndex = distribution.Key,
                TotalDeposited = distribution.Value.TotalDeposited,
                TotalWithdrawn = distribution.Value.TotalWithdrawn
            });
        }

        public async Task<IEnumerable<BrokerAccountMonthTransferDto>> GetYearTransfersHistory(Guid brokerAccountId, int year)
        {
            //TODO: Add db year filter

            var transfers = await _fundsTransferService.GetAll(brokerAccountId);
            var filteredTransfers = transfers.Where(transfer => transfer.Date.Year == year);

            var distributions = Enumerable.Range(1, 12).ToDictionary(k => k, v => new TransfersHistory());

            foreach (var transfer in filteredTransfers)
            {
                var distribution = distributions[transfer.Date.Month];

                if (transfer.Income)
                {
                    distribution.TotalDeposited += transfer.Amount;
                }
                else
                {
                    distribution.TotalWithdrawn += transfer.Amount;
                }
            }

            return distributions.Select(distribution => new BrokerAccountMonthTransferDto()
            {
                MonthIndex = distribution.Key,
                TotalDeposited = distribution.Value.TotalDeposited,
                TotalWithdrawn = distribution.Value.TotalWithdrawn
            });
        }

        private async Task<BrokerAccountTransfersStatsDto> GetTransfersStats(Guid brokerAccountId)
        {
            var transfers = (await _fundsTransferService.GetAll(brokerAccountId)).ToList();

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

        private class TransfersHistory
        {
            public decimal TotalDeposited { get; set; }
            public decimal TotalWithdrawn { get; set; }
        }

    }
}
