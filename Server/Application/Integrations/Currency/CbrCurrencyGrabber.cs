using MoneyManager.Application.Integrations.Currency.Models;
using MoneyManager.Application.Interfaces.Integrations.Currency;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace MoneyManager.Application.Integrations.Currency
{
    public class CbrCurrencyGrabber: ICurrencyGrabber
    {
        private const string CbrBaseCurrency = "RUB";

        private readonly IHttpClientFactory _httpClientFactory;

        public CbrCurrencyGrabber(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<Dictionary<string, decimal>> GetRates(string mainCurrency, HashSet<string> requestedCurrencies)
        {
            var httpClient = _httpClientFactory.CreateClient();
            var response = await httpClient.GetAsync("https://www.cbr-xml-daily.ru/latest.js");
            var currenciesInfo = await response.Content.ReadFromJsonAsync<CbrCurrencyResponse>();

            if (mainCurrency == CbrBaseCurrency)
            {
                return currenciesInfo
                    .Rates.Where(rate => requestedCurrencies.Contains(rate.Key))
                    .ToDictionary(rate => rate.Key, rate => 1 / rate.Value);
            }

            var mainCurrencyRate = currenciesInfo.Rates.FirstOrDefault(rate =>
                rate.Key == mainCurrency);

            var rates = currenciesInfo
                .Rates.Where(rate => requestedCurrencies.Contains(rate.Key))
                .ToDictionary(rate => rate.Key, rate => 
                    rate.Value / mainCurrencyRate.Value);
            rates[CbrBaseCurrency] = 1 / mainCurrencyRate.Value;
            return rates;
        }
    }
}
