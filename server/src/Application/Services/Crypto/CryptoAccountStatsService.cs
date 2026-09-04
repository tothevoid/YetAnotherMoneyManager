using Audex.Application.DTO.Crypto;
using Audex.Application.DTO.Dashboard;
using Audex.Application.Interfaces.Crypto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Audex.Application.Services.Crypto
{
    public class CryptoAccountStatsService : ICryptoAccountStatsService
    {
        private readonly ICryptoAccountCryptocurrencyService _cryptoAccountCryptocurrencyService;
        private readonly ICryptocurrencyService _cryptocurrencyService;

        public CryptoAccountStatsService(
            ICryptoAccountCryptocurrencyService cryptoAccountCryptocurrencyService,
            ICryptocurrencyService cryptocurrencyService)
        {
            _cryptoAccountCryptocurrencyService = cryptoAccountCryptocurrencyService;
            _cryptocurrencyService = cryptocurrencyService;
        }

        public Task<CryptoAccountStatsDto> GetStatsAsync() =>
            BuildStatsAsync(null);

        public Task<CryptoAccountStatsDto> GetStatsByCryptoAccountAsync(Guid cryptoAccountId) =>
            BuildStatsAsync(cryptoAccountId);

        private async Task<CryptoAccountStatsDto> BuildStatsAsync(Guid? cryptoAccountId)
        {
            var items = (cryptoAccountId.HasValue
                ? await _cryptoAccountCryptocurrencyService.GetByCryptoAccountAsync(cryptoAccountId.Value)
                : await _cryptoAccountCryptocurrencyService.GetAllAsync()).ToList();

            if (items.Count == 0)
            {
                return new CryptoAccountStatsDto
                {
                    CryptoDistribution = new List<DistributionDto>(),
                    AccountsDistribution = new List<DistributionDto>()
                };
            }

            var baseCurrency = await _cryptocurrencyService.GetBaseCurrencyAsync();
            var baseCurrencyCode = baseCurrency.Name;
            var baseRate = baseCurrency.Rate;

            var cryptoDistribution = CalculateDistribution(
                items,
                c => c.Cryptocurrency,
                c => $"{c.Name} ({c.Symbol})",
                baseRate,
                baseCurrencyCode);

            var accountsDistribution = new List<DistributionDto>();

            if (!cryptoAccountId.HasValue)
            {
                accountsDistribution = CalculateDistribution(
                    items,
                    c => c.CryptoAccount,
                    a => a.Name,
                    baseRate,
                    baseCurrencyCode);
            }

            return new CryptoAccountStatsDto
            {
                CryptoDistribution = cryptoDistribution,
                AccountsDistribution = accountsDistribution
            };
        }

        private static List<DistributionDto> CalculateDistribution<TKey>(
            IEnumerable<CryptoAccountCryptocurrencyDto> items,
            Func<CryptoAccountCryptocurrencyDto, TKey> keySelector,
            Func<TKey, string> nameSelector,
            decimal baseRate,
            string baseCurrencyCode)
        {
            return items
                .GroupBy(keySelector)
                .Select(g =>
                {
                    var amount = g.Sum(c => c.Quantity * c.Cryptocurrency.Price);
                    return new DistributionDto
                    {
                        Name = nameSelector(g.Key),
                        Currency = baseCurrencyCode,
                        Amount = amount,
                        ConvertedAmount = amount * baseRate
                    };
                })
                .OrderByDescending(d => d.Amount)
                .ToList();
        }
    }
}
