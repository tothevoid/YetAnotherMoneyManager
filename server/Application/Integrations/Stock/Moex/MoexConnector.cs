using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Drawing.Charts;
using MoneyManager.Application.DTO.Securities;
using MoneyManager.Application.Integrations.Stock.Moex.Model;
using MoneyManager.Application.Interfaces.Integrations.Stock;
using MoneyManager.Application.Interfaces.Securities;
using MoneyManager.Infrastructure.Constants;

namespace MoneyManager.Application.Integrations.Stock.Moex
{
    public class MoexConnector: IStockConnector
    {
        private IHttpClientFactory _httpClientFactory;

        public MoexConnector(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IEnumerable<SecurityHistoryValueDto>> GetTickerHistory(SecurityDTO security, DateOnly from, DateOnly to)
        {
            var httpClient = _httpClientFactory.CreateClient();

            string query = security.TypeId == SecurityTypeConstants.PreciousMetal ?
                MoexUrlFactory.GetCurrencyHistoricalQuery(security.Ticker, from, to):
                MoexUrlFactory.GetHistoricalQuery(security.Ticker, from, to);

            return await GetHistory(query, httpClient);
        }

        public async Task<IEnumerable<MarketDataRow>> GetExtendedValuesByTickers(IEnumerable<SecurityDTO> securities)
        {
            var httpClient = _httpClientFactory.CreateClient();

            var (baseSecurities, currencySecurities) = SplitTickersByType(securities);

            var result = new List<MarketDataRow>();

            if (baseSecurities.Any())
            {
                var query = MoexUrlFactory.GetFullSecuritiesQuery(baseSecurities);
                result.AddRange(await FetchAndApplySecuritiesAsync(httpClient, query));
            }
           
            if (currencySecurities.Any())
            {
                var query = MoexUrlFactory.GetFullCurrencySecuritiesQuery(currencySecurities);
                result.AddRange(await FetchAndApplySecuritiesAsync(httpClient, query));
            }

            return result;
        }

        public async Task<IEnumerable<MarketDataRow>> GetValuesByTickers(IEnumerable<SecurityDTO> securities)
        {
            var httpClient = _httpClientFactory.CreateClient();

            var (baseSecurities, currencySecurities) = SplitTickersByType(securities);

            var dataRows = new List<MarketDataRow>();

            if (baseSecurities.Any())
            {
                var baseSecuritiesQuery = MoexUrlFactory.GetBaseSecuritiesQuery(baseSecurities);
                dataRows.AddRange(await FetchMarketDataRowsAsync(httpClient, baseSecuritiesQuery));
            }

            if (currencySecurities.Any())
            {
                var currencySecuritiesQuery = MoexUrlFactory.GetBaseCurrencySecuritiesQuery(currencySecurities);
                dataRows.AddRange(await FetchMarketDataRowsAsync(httpClient, currencySecuritiesQuery));
            }

            return dataRows;
        }

        private async Task<IEnumerable<MarketDataRow>> FetchMarketDataRowsAsync(HttpClient httpClient, string query)
        {
            var tickersData = await FetchTickersDataAsync(httpClient, query);

            return ParseMarketDataRows(tickersData.MarketData.Columns, tickersData.MarketData);
        }

        private async Task<MoexResponse> FetchTickersDataAsync(HttpClient httpClient, string query)
        {
            var result = await httpClient.GetAsync(query);
            return await result.Content.ReadFromJsonAsync<MoexResponse>();
        }

        private async Task<IEnumerable<MarketDataRow>> FetchAndApplySecuritiesAsync(HttpClient httpClient, string query)
        {
            var tickersData = await FetchTickersDataAsync(httpClient, query);

            var marketData = ParseMarketDataRows(tickersData.MarketData.Columns, tickersData.MarketData)
                .ToList();

            return ParseAndApplySecuritiesRows(marketData, tickersData.Securities.Columns, tickersData.Securities);
        }

        private (HashSet<string> baseSecurities, HashSet<string> currencySecurities) SplitTickersByType(IEnumerable<dynamic> securities)
        {
            var baseSecurities = new HashSet<string>();
            var currencySecurities = new HashSet<string>();

            if (securities == null)
            {
                return (baseSecurities, currencySecurities);
            }

            foreach (var security in securities)
            {
                if (security.TypeId == SecurityTypeConstants.PreciousMetal)
                {
                    currencySecurities.Add(security.Ticker);
                }
                else
                {
                    baseSecurities.Add(security.Ticker);
                }
            }

            return (baseSecurities, currencySecurities);
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
            var lowIndex = columnsIndexes["LOW"];
            var highIndex = columnsIndexes["HIGH"];

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
                        MarketPrice = TryGetDecimalValue(row[marketPriceIndex]),
                        Open = TryGetDecimalValue(row[openIndex]),
                        Low = TryGetDecimalValue(row[lowIndex], 0),
                        High = TryGetDecimalValue(row[highIndex], 0)
                    }
                )
                .OrderBy(row => GetBoardPriority(row.BoardId));
        }

        private decimal? TryGetDecimalValue(object value)
        {
            return value != null
                ? Convert.ToDecimal(value.ToString(), CultureInfo.InvariantCulture)
                : null;
        }

        private decimal TryGetDecimalValue(object value, decimal defaultValue)
        {
            return TryGetDecimalValue(value) ?? defaultValue;
        }

        private IEnumerable<MarketDataRow> ParseAndApplySecuritiesRows(IEnumerable<MarketDataRow> marketDataRows, 
            IEnumerable<string> columns,
            DynamicMoexResponseObject securities)
        {
            var columnsIndexes = GetColumnIndexMapping(columns.ToArray());

            var tickerIndex = columnsIndexes["SECID"];
            var boardIdIndex = columnsIndexes["BOARDID"];
            var prevPrice = columnsIndexes["PREVPRICE"];

            var securityRows =  securities.Data
                .Select(row =>
                    new SecurityRow()
                    {
                        Ticker = Convert.ToString(row[tickerIndex]),
                        BoardId = Convert.ToString(row[boardIdIndex]),
                        PrevPrice = TryGetDecimalValue(row[prevPrice])
                    }
                )
                .OrderBy(row => GetBoardPriority(row.BoardId))
                .ToDictionary(key => key.GetUniqueKey(), value => value);

            foreach (var marketDataRow in marketDataRows)
            {
                var key = marketDataRow.GetUniqueKey();
                if (!securityRows.ContainsKey(key))
                {
                    continue;
                }

                marketDataRow.PrevPrice = securityRows[key].PrevPrice;
            }

            return marketDataRows;
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
                case "CETS":
                    return 9;
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
                if (value == default)
                {
                    continue;
                }

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
