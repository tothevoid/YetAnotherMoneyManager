using AutoMapper;
using MoneyManager.Application.DTO.Brokers;
using MoneyManager.Application.Interfaces.Brokers;
using MoneyManager.Application.Interfaces.Integrations.Stock;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Markup;
using MoneyManager.Application.Interfaces.User;
using MoneyManager.Infrastructure.Entities.Brokers;

namespace MoneyManager.Application.Services.Brokers
{
    public class BrokerAccountSummaryService: IBrokerAccountSummaryService
    {
        private readonly IUserProfileService _userProfileService;
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
            IStockConnector stockConnector,
            IUserProfileService userProfileService)
        {
            _brokerAccountSecurityService = brokerAccountSecuritySecurityService;
            _fundsTransferService = fundsTransferService;
            _stockConnector = stockConnector;
            _brokerAccountService = brokerAccountService;
            _dividendPaymentService = dividendPaymentService;
            _userProfileService = userProfileService;
        }

        public async Task<BrokerAccountSummaryDto> GetSummary()
        {
            var transfers = (await _fundsTransferService.GetAll()).ToList();

            return new BrokerAccountSummaryDto()
            {
                TransferStats = GetTransfersStats(transfers),
                BrokerAccountStats = await GetBrokerAccountsStats()
            };
        }

        public async Task<BrokerAccountSummaryDto> GetSummaryByBrokerAccount(Guid brokerAccountId)
        {
            var transfers = (await _fundsTransferService.GetAll(brokerAccountId)).ToList();

            return new BrokerAccountSummaryDto()
            {
                TransferStats = GetTransfersStats(transfers),
                BrokerAccountStats = await GetBrokerAccountStats(brokerAccountId)
            };
        }

        public async Task<BrokerAccountDailyStatsDto> GetDailyStatsByBrokerAccount(Guid brokerAccountId)
        {
            var securities = (await _brokerAccountSecurityService
                .GetByBrokerAccount(brokerAccountId)).ToList();
            var brokerAccount = await _brokerAccountService.GetById(brokerAccountId);

            return await GetDailyStats(securities, brokerAccount.MainCurrencyAmount);
        }

        public async Task<BrokerAccountDailyStatsDto> GetDailyStats()
        {
            var brokerAccountSecurities = (await _brokerAccountSecurityService.GetAll()).ToList();
            var brokerAccounts = (await _brokerAccountService.GetAll()).ToList();

            var amount = brokerAccounts.Sum(account => account.MainCurrencyAmount);

            return await GetDailyStats(brokerAccountSecurities, amount);
        }

        private async Task<BrokerAccountDailyStatsDto> GetDailyStats(List<BrokerAccountSecurityDTO> brokerAccountSecurities, 
            decimal portfolioMainCurrencyValue)
        {
            var securitySummaries = await GetSummaryPortfolioSecurities(brokerAccountSecurities);

            // TODO: Possible different currencies that cant be combined
            decimal startPortfolioValue = portfolioMainCurrencyValue;
            decimal currentPortfolioValue = portfolioMainCurrencyValue;

            foreach (var securitySummary in securitySummaries)
            {
                var currentPrice = securitySummary.CurrentPrice;
                var startPrice = securitySummary.StartPrice;

                startPortfolioValue += startPrice * securitySummary.Quantity;
                currentPortfolioValue += currentPrice * securitySummary.Quantity;
            }

            return new BrokerAccountDailyStatsDto()
            {
                FetchDate = DateTime.UtcNow,
                BrokerAccountDailySecurityStats = securitySummaries.OrderBy(stat => stat.CurrentPrice - stat.PreviousDayClosePrice),
                CurrentPortfolioValue = currentPortfolioValue,
                StartPortfolioValue = startPortfolioValue
            };
        }

        private async Task<List<BrokerAccountDailySecurityStatsDto>> GetSummaryPortfolioSecurities(List<BrokerAccountSecurityDTO> brokerAccountSecurities)
        {
            var securities = brokerAccountSecurities
                .Select(brokerAccountSecurity => brokerAccountSecurity.Security)
                .DistinctBy(security => security.Ticker)
                .ToList();

            var tickers = securities.ToDictionary(key => key.Ticker, value => value);

            var securitiesQuantities = new Dictionary<Guid, int>();

            foreach (var brokerAccountSecurity in brokerAccountSecurities)
            {
                var securityId = brokerAccountSecurity.SecurityId;
                if (securitiesQuantities.ContainsKey(securityId))
                {
                    securitiesQuantities[securityId] += brokerAccountSecurity.Quantity;
                }
                else
                {
                    securitiesQuantities.Add(securityId, brokerAccountSecurity.Quantity);
                }
            }

            var marketValues = (await _stockConnector
                .GetExtendedValuesByTickers(tickers.Keys));

            var securityStats = new List<BrokerAccountDailySecurityStatsDto>();

            var handledTickers = new HashSet<string>();

            foreach (var marketValue in marketValues)
            {
                if (handledTickers.Contains(marketValue.Ticker))
                {
                    continue;
                }

                var security = tickers[marketValue.Ticker];

                var currentPrice = marketValue.GetLastValue();

                securityStats.Add(new BrokerAccountDailySecurityStatsDto()
                {
                    CurrentPrice = currentPrice,
                    Security = security,
                    StartPrice = marketValue.PrevPrice ?? marketValue.Open ?? 0,
                    MinPrice = marketValue.Low,
                    MaxPrice = marketValue.High,
                    PreviousDayClosePrice = marketValue.PrevPrice ?? 0,
                    Quantity = securitiesQuantities[security.Id]
                });
                handledTickers.Add(marketValue.Ticker);
            }

            return securityStats;
        }

        public async Task<IEnumerable<BrokerAccountDayTransferDto>> GetMonthTransfersHistory(int month, int year)
        {
            var transfers = (await _fundsTransferService.GetAll()).ToList();

            return GetMonthTransfersHistoryByBrokerAccount(transfers, month, year);
        }

        public async Task<IEnumerable<BrokerAccountDayTransferDto>> GetMonthTransfersHistoryByBrokerAccount(Guid brokerAccountId, int month, int year)
        {
            var transfers = (await _fundsTransferService.GetAll(brokerAccountId)).ToList();

            return GetMonthTransfersHistoryByBrokerAccount(transfers, month, year);
        }

        private IEnumerable<BrokerAccountDayTransferDto> GetMonthTransfersHistoryByBrokerAccount(List<BrokerAccountFundsTransferDto> transfers,
            int month, int year)
        {
            //TODO: Add db month and year filter
            //TODO: fix GetMonthTransfersHistory & GetYearTransfersHistory code duplication
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

        public async Task<IEnumerable<BrokerAccountMonthTransferDto>> GetYearTransfersHistory(int year)
        {
            var transfers = (await _fundsTransferService.GetAll()).ToList();
            return GetYearTransfersHistoryByBrokerAccount(transfers, year);
        }

        public async Task<IEnumerable<BrokerAccountMonthTransferDto>> GetYearTransfersHistoryByBrokerAccount(Guid brokerAccountId, int year)
        {
            var transfers = (await _fundsTransferService.GetAll(brokerAccountId)).ToList();
            return GetYearTransfersHistoryByBrokerAccount(transfers, year);
        }

        private IEnumerable<BrokerAccountMonthTransferDto> GetYearTransfersHistoryByBrokerAccount(List<BrokerAccountFundsTransferDto> transfers, 
            int year)
        {
            //TODO: Add db year filter
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

        private BrokerAccountTransfersStatsDto GetTransfersStats(List<BrokerAccountFundsTransferDto> transfers)
        {
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

        private async Task<BrokerAccountStatsDto> GetBrokerAccountsStats()
        {
            var portfolioValues = await _brokerAccountService.GetPortfolioValues();
            var totalDividends = await _dividendPaymentService.GetEarnings();

            return new BrokerAccountStatsDto()
            {
                CurrentValue = portfolioValues.CurrentValue,
                InvestedValue = portfolioValues.InitialValue,
                TotalDividendsValue = totalDividends
            };
        }

        private async Task<BrokerAccountStatsDto> GetBrokerAccountStats(Guid brokerAccountId)
        {
            var portfolioValues = await _brokerAccountService.GetPortfolioValuesByBrokerAccount(brokerAccountId);
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
