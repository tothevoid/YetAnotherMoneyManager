using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using MoneyManager.Application.DTO.Securities;
using MoneyManager.Application.Integrations.Stock.Moex.Model;
using MoneyManager.Application.Interfaces.Integrations.Stock;

namespace MoneyManager.Application.Integrations.Stock.Moex
{
    public class MoexConnector: IStockConnector
    {
        private IHttpClientFactory _httpClientFactory;

        public MoexConnector(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IEnumerable<SecurityHistoryValueDto>> GetTickerHistory(string ticker, DateOnly from, DateOnly to)
        {
            var httpClient = _httpClientFactory.CreateClient();

            var query = MoexUrlFactory.GetHistoricalQuery(ticker, from, to);

            return await GetHistory(query, httpClient);
        }

        public Task<FullSecurityData> GetValuesByTickersInRange(IEnumerable<string> tickers, DateOnly from, DateOnly to)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<MarketDataRow>> GetValuesByTickers(IEnumerable<string> tickers)
        {
            var httpClient = _httpClientFactory.CreateClient();

            var query = MoexUrlFactory.GetBaseSecuritiesQuery(tickers);

            var result = await httpClient.GetAsync(query);
            var tickersData = await result.Content.ReadFromJsonAsync<MoexResponse>();

            return ParseMarketDataRows(tickersData.MarketData.Columns, tickersData.MarketData);
        }

        public async Task<FullSecurityData> GetFullValuesByTickersInRange(IEnumerable<string> tickers,
            DateOnly from, DateOnly to)
        {
            var httpClient = _httpClientFactory.CreateClient();

            var query = MoexUrlFactory.GetFullSecuritiesQuery(tickers, from, to);
            var result = await httpClient.GetAsync(query);

            var tickersData = await result.Content.ReadFromJsonAsync<MoexResponse>();

            return new FullSecurityData()
            {
                Security = ParseSecuritiesRows(tickersData.MarketData.Columns, tickersData.MarketData),
                MarketData = ParseMarketDataRows(tickersData.MarketData.Columns, tickersData.Securities)
            };
        }

        private IEnumerable<MarketDataRow> ParseMarketDataRows(IEnumerable<string> columns, DynamicMoexResponseObject marketData)
        {
            var columnsIndexes = GetColumnIndexMapping(columns.ToArray());

            var boardIdIndex = columnsIndexes["BOARDID"];
            var openIndex = columnsIndexes["OPEN"];
            var tickerIndex = columnsIndexes["SECID"];
            var lastValueIndex = columnsIndexes["LAST"];
            var dateIndex = columnsIndexes["SYSTIME"];
            var marketPriceIndex = columnsIndexes["MARKETPRICE"];

            return marketData.Data
                .Select(row =>
                    new MarketDataRow()
                    {
                        Ticker = Convert.ToString(row[tickerIndex]),
                        BoardId = Convert.ToString(row[boardIdIndex]),
                        LastValue = row[lastValueIndex] != null
                            ? Convert.ToDecimal(row[lastValueIndex].ToString(), CultureInfo.InvariantCulture)
                            : null,
                        Date = Convert.ToDateTime(row[dateIndex].ToString()),
                        MarketPrice = row[marketPriceIndex] != null
                            ? Convert.ToDecimal(row[marketPriceIndex].ToString(), CultureInfo.InvariantCulture)
                            : null,
                        Open = row[openIndex] != null ?
                            Convert.ToDecimal(row[openIndex].ToString(), CultureInfo.InvariantCulture)
                            : null
                    }
                )
                .OrderBy(row => GetBoardPriority(row.BoardId));
        }

        private IEnumerable<SecurityRow> ParseSecuritiesRows(IEnumerable<string> columns, DynamicMoexResponseObject marketData)
        {
            var columnsIndexes = GetColumnIndexMapping(columns.ToArray());

            var tickerIndex = columnsIndexes["SECID"];
            var updateTimeIndex = columnsIndexes["UPDATETIME"];
            var boardIdIndex = columnsIndexes["BOARDID"];
            var prevPrice = columnsIndexes["PREVPRICE"];

            return marketData.Data
                .Select(row =>
                    new SecurityRow()
                    {
                        Ticker = Convert.ToString(row[tickerIndex]),
                        BoardId = Convert.ToString(row[boardIdIndex]),
                        UpdateTime = Convert.ToDateTime(row[updateTimeIndex]),
                        PrevPrice = row[prevPrice] != null
                            ? Convert.ToDecimal(row[prevPrice], CultureInfo.InvariantCulture)
                            : null
                    }
                )
                .OrderBy(row => GetBoardPriority(row.BoardId));
        }

        private int GetBoardPriority(string boardId)
        {
            // Lower -> higher priority
            switch (boardId)
            {
                case "TQBR":
                    return 1;
                case "TQTF":
                    return 2;
                case "TQPI":
                    return 3;
                case "SMAL":
                    return 4;
                case "SPEQ":
                    return 5;
                case "EQRP":
                    return 6;
                case "EQOB":
                    return 7;
                case "TQOD":
                    return 8;
                default:
                    return 100;
            }
        }

        private async Task<IEnumerable<SecurityHistoryValueDto>> GetHistory(string query, HttpClient httpClient)
        {
            var result = await httpClient.GetAsync(query);
            var tickerHistory = await result.Content.ReadFromJsonAsync<MoexHistoryResponse>();

            var columnsIndexes = GetColumnIndexMapping(tickerHistory.History.Columns.ToArray());

            var tradeDate = columnsIndexes["TRADEDATE"];
            var closeValue = columnsIndexes["CLOSE"];

            var historyValues = new List<SecurityHistoryValueDto>();

            foreach (var marketData in tickerHistory.History.Data)
            {
                var rawValue = marketData[closeValue];

                if (rawValue == null)
                {
                    continue;
                }

                var value = Convert.ToDecimal(rawValue.ToString(), CultureInfo.InvariantCulture);
                var date = Convert.ToDateTime(marketData[tradeDate].ToString());

                historyValues.Add(new SecurityHistoryValueDto() { Value = value, Date = DateOnly.FromDateTime(date) });
            }

            return historyValues;
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
}
