using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using MoneyManager.Application.Integrations.Stock.Moex;
using MoneyManager.Application.Integrations.Stock.Moex.Model;
using MoneyManager.Application.Interfaces.Integrations.Stock;

namespace MoneyManager.Application.Integrations.Stock
{
    public class MoexConnector: IStockConnector
    {
        private IHttpClientFactory _httpClientFactory;

        public MoexConnector(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<Dictionary<string, decimal>> GetValuesByTickers(IEnumerable<string> tickers)
        {
            var httpClient = _httpClientFactory.CreateClient();

            var query = new MoexUrlBuilder()
                .BuildSecuritiesQuery(tickers);

            var result = await httpClient.GetAsync(query);
            var tickersData = await result.Content.ReadFromJsonAsync<MoexResponse>();

            var columnsIndexes = GetColumnIndexMapping(tickersData.MarketData.Columns.ToArray());

            var tickerIndex = columnsIndexes["SECID"];
            var valueIndex = columnsIndexes["LAST"];
            var dateIndex = columnsIndexes["SYSTIME"];

            var marketValues = new List<MarketValue>();

            foreach (var marketData in tickersData.MarketData.Data)
            {
                var ticker = Convert.ToString(marketData[tickerIndex]);

                var rawValue = marketData[valueIndex];
                if (rawValue == null)
                {
                    continue;
                }

                var value = Convert.ToDecimal(rawValue.ToString(), CultureInfo.InvariantCulture);
                var date = Convert.ToDateTime(marketData[dateIndex].ToString());

                marketValues.Add(new MarketValue() { Ticker = ticker, Value = value, Date = date });
            }

            return marketValues.OrderBy(marketValue => marketValue.Date)
                .DistinctBy(marketValue => marketValue.Ticker)
                .ToDictionary((marketValue) => marketValue.Ticker, (marketValue) => marketValue.Value);
        }

        private Dictionary<string, int> GetColumnIndexMapping(string[] columns)
        {
            var columnsIndexes = new Dictionary<string, int>();

            for (int i = 0; i < columns.Length; i++)
            {
                columnsIndexes[columns[i]] = i;
            }

            return columnsIndexes;
        }
    }

    public class MarketValue
    {
        public string Ticker { get; init; }

        public decimal Value { get; init; }

        public DateTime Date { get; init; }
    }
}
