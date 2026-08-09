using MoneyManager.Application.Interfaces.Brokers;
using MoneyManager.Application.Interfaces.Securities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoneyManager.Application.Services.Brokers
{
    public class BrokerAccountPortfolioHistoryService : IBrokerAccountPortfolioHistoryService
    {
        private readonly ISecurityTransactionService _securityTransactionService;
        private readonly IBrokerAccountFundsTransferService _brokerAccountFundsTransferService;
        private readonly IDividendPaymentService _dividendPaymentService;
        private readonly IBrokerAccountTaxDeductionService _taxDeductionService;
        private readonly ISecurityService _securityService;

        public BrokerAccountPortfolioHistoryService(
            ISecurityTransactionService securityTransactionService,
            IBrokerAccountFundsTransferService brokerAccountFundsTransferService,
            IDividendPaymentService dividendPaymentService,
            IBrokerAccountTaxDeductionService taxDeductionService,
            ISecurityService securityService)
        {
            _securityTransactionService = securityTransactionService;
            _brokerAccountFundsTransferService = brokerAccountFundsTransferService;
            _dividendPaymentService = dividendPaymentService;
            _taxDeductionService = taxDeductionService;
            _securityService = securityService;
        }

        public async Task<BrokerAccountPortfolioHistoryDto> GetAll(DateOnly date)
        {
            return await GetHistory(date, null);
        }

        public async Task<BrokerAccountPortfolioHistoryDto> GetByBrokerAccount(DateOnly date, Guid brokerAccountId)
        {
            return await GetHistory(date, brokerAccountId);
        }

        private async Task<BrokerAccountPortfolioHistoryDto> GetHistory(DateOnly date, Guid? brokerAccountId)
        {
            //TODO: Task.WhenAll
            var taxDeductionsSum = await GetTotalTaxDeduction(date, brokerAccountId);
            var dividendPaymentsSum = await GetTotalDividendPayments(date, brokerAccountId);
            var (totalDeposited, totalWithdrawn) = await GetTotalTransfers(date, brokerAccountId);
            var securitiesStats = await GetSecuritiesStats(date, brokerAccountId);

            var totalPurchased = securitiesStats.Sum(security => security.Value.PurchasePriceSum);
            var totalSold = securitiesStats.Sum(security => security.Value.SellPriceSum);

            Console.WriteLine("Purch:" + totalPurchased);
            Console.WriteLine("Sold:" + totalSold);

            decimal totalPositive = totalDeposited + dividendPaymentsSum + totalSold;
            decimal totalNegative = totalWithdrawn + totalPurchased;
            decimal mainCurrencyValue = totalPositive - totalNegative;

            Console.WriteLine("Dep:" + totalDeposited);

            decimal portfolioValue = mainCurrencyValue;

            foreach (var securitiesStat in securitiesStats)
            {
                //TODO: Fetch by date
                var security = await _securityService.FindByTicker(securitiesStat.Key);

                portfolioValue += securitiesStat.Value.ActualQuantity * security.ActualPrice;

                Console.WriteLine($"ticker: {security.Ticker}. Total: {securitiesStat.Value.ActualQuantity * security.ActualPrice}");
            }

            return new BrokerAccountPortfolioHistoryDto
            {
                Date = date,
                TotalDividends = dividendPaymentsSum,
                TotalTaxDeduction = taxDeductionsSum,
                TotalDeposited = totalDeposited,
                TotalWithdraw = totalWithdrawn,

                MainCurrencyAmount = mainCurrencyValue,
                PortfolioValue = portfolioValue,
                ProfitAndLoss = portfolioValue + taxDeductionsSum - (totalDeposited - totalWithdrawn),
            };
        }

        private async Task<decimal> GetTotalTaxDeduction(DateOnly date, Guid? brokerAccountId)
        {
            return await _taxDeductionService.GetSumTillSpecificDate(date, brokerAccountId);
        }

        private async Task<decimal> GetTotalDividendPayments(DateOnly date, Guid? brokerAccountId)
        {
            return await _dividendPaymentService.GetSumTillSpecificDate(date, brokerAccountId);
        }

        private async Task<(decimal deposited, decimal withdrawn)> GetTotalTransfers(DateOnly date, Guid? brokerAccountId)
        {
            return await _brokerAccountFundsTransferService.GetSumTillSpecificDate(date, brokerAccountId);
        }

        private async Task<Dictionary<string, SecurityTransactionsSummary>> GetSecuritiesStats(DateOnly date, Guid? brokerAccountId)
        {
            return await _securityTransactionService.GetSummaryTillSpecificDate(date, brokerAccountId);
        }
    }

    public class BrokerAccountPortfolioHistoryDto
    {
        public DateOnly Date { get; set; }

        public decimal MainCurrencyAmount { get; set; }

        public decimal PortfolioValue { get; set; }

        public decimal TotalDividends { get; set; }

        public decimal TotalTaxDeduction { get; set; }

        public decimal TotalDeposited { get; set; }

        public decimal TotalWithdraw { get; set; }

        public decimal ProfitAndLoss { get; set; }
    }
}
