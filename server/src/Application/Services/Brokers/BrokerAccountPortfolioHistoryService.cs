using Audex.Application.DTO.Brokers;
using Audex.Application.DTO.Securities;
using Audex.Application.Interfaces.Brokers;
using Audex.Application.Interfaces.Integrations.Stock;
using Audex.Application.Interfaces.Securities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Audex.Application.Services.Brokers
{
    public class BrokerAccountPortfolioHistoryService(
        ISecurityTransactionService securityTransactionService,
        IBrokerAccountFundsTransferService brokerAccountFundsTransferService,
        IDividendPaymentService dividendPaymentService,
        IBrokerAccountTaxDeductionService taxDeductionService,
        ISecurityService securityService,
        IStockConnector stockConnector) : IBrokerAccountPortfolioHistoryService
    {
        private readonly ISecurityTransactionService _securityTransactionService = securityTransactionService;
        private readonly IBrokerAccountFundsTransferService _brokerAccountFundsTransferService = brokerAccountFundsTransferService;
        private readonly IDividendPaymentService _dividendPaymentService = dividendPaymentService;
        private readonly IBrokerAccountTaxDeductionService _taxDeductionService = taxDeductionService;
        private readonly ISecurityService _securityService = securityService;
        private readonly IStockConnector _stockConnector = stockConnector;

        public async Task<BrokerAccountPortfolioHistoryDto> GetAllAsync(DateOnly date)
        {
            return await GetHistory(date, null);
        }

        public async Task<BrokerAccountPortfolioHistoryDto> GetByBrokerAccountAsync(DateOnly date, Guid brokerAccountId)
        {
            return await GetHistory(date, brokerAccountId);
        }

        private async Task<BrokerAccountPortfolioHistoryDto> GetHistory(DateOnly date, Guid? brokerAccountId)
        {
            var taxDeductionsSum = await GetTotalTaxDeduction(date, brokerAccountId);
            var dividendPaymentsSum = await GetTotalDividendPayments(date, brokerAccountId);
            var (totalDeposited, totalWithdrawn) = await GetTotalTransfers(date, brokerAccountId);
            var securitiesStats = await GetSecuritiesStats(date, brokerAccountId);

            var totalPurchased = securitiesStats.Sum(security => security.Value.PurchasePriceSum);
            var totalSold = securitiesStats.Sum(security => security.Value.SellPriceSum);

            decimal totalPositive = totalDeposited + dividendPaymentsSum + totalSold;
            decimal totalNegative = totalWithdrawn + totalPurchased;
            decimal mainCurrencyValue = totalPositive - totalNegative;

            decimal securitiesValue = await CalculateSecuritiesValue(securitiesStats, date);
            decimal portfolioValue = mainCurrencyValue + securitiesValue;

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

        private async Task<decimal> CalculateSecuritiesValue(Dictionary<string, SecurityTransactionsSummaryDto> securitiesStats, DateOnly date)
        {
            if (securitiesStats == null || securitiesStats.Count == 0)
            {
                return 0m;
            }

            var securitiesList = await _securityService.FindByTickersAsync(securitiesStats.Keys);
            var securitiesMap = securitiesList.ToDictionary(s => s.Ticker.ToLower(), s => s);

            using var semaphore = new SemaphoreSlim(5);
            var priceTasks = securitiesStats.Select(async securitiesStat =>
            {
                await semaphore.WaitAsync();
                try
                {
                    if (!securitiesMap.TryGetValue(securitiesStat.Key.ToLower(), out var security) || security == null)
                    {
                        return (securitiesStat.Key, 0m);
                    }

                    decimal price = await GetSecurityPriceAtDate(security, date);
                    return (securitiesStat.Key, price);
                }
                finally
                {
                    semaphore.Release();
                }
            });

            var priceResults = await Task.WhenAll(priceTasks);
            var priceMap = priceResults.ToDictionary(r => r.Key, r => r.Item2);

            decimal totalSecuritiesValue = 0m;
            foreach (var securitiesStat in securitiesStats)
            {
                decimal price = priceMap.TryGetValue(securitiesStat.Key, out var securityPrice) ? securityPrice : 0m;
                totalSecuritiesValue += securitiesStat.Value.ActualQuantity * price;
            }

            return totalSecuritiesValue;
        }

        private async Task<decimal> GetSecurityPriceAtDate(SecurityDto security, DateOnly date)
        {
            try
            {
                // TODO: handle long holidays (>7 days) case
                var history = await _stockConnector.GetCandlesAsync(security, date.AddDays(-7), date);
                var lastHistoryValue = history?.OrderBy(h => h.End).LastOrDefault();
                if (lastHistoryValue != null && lastHistoryValue.Close > 0)
                {
                    return lastHistoryValue.Close;
                }
            }
            catch
            {
                Console.WriteLine($"'{security.Ticker}' history fetch error");
                // Return 0m fallback when history fetch fails or missing
            }

            return 0m;
        }

        private async Task<decimal> GetTotalTaxDeduction(DateOnly date, Guid? brokerAccountId)
        {
            return await _taxDeductionService.GetSumTillSpecificDateAsync(date, brokerAccountId);
        }

        private async Task<decimal> GetTotalDividendPayments(DateOnly date, Guid? brokerAccountId)
        {
            return await _dividendPaymentService.GetSumTillSpecificDateAsync(date, brokerAccountId);
        }

        private async Task<(decimal deposited, decimal withdrawn)> GetTotalTransfers(DateOnly date, Guid? brokerAccountId)
        {
            return await _brokerAccountFundsTransferService.GetSumTillSpecificDateAsync(date, brokerAccountId);
        }

        private async Task<Dictionary<string, SecurityTransactionsSummaryDto>> GetSecuritiesStats(DateOnly date, Guid? brokerAccountId)
        {
            return await _securityTransactionService.GetSummaryTillSpecificDateAsync(date, brokerAccountId);
        }
    }
}
