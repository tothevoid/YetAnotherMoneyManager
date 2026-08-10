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
    public class MoexConnector(IHttpClientFactory httpClientFactory) : IStockConnector
    {
        public async Task<IEnumerable<SecurityHistoryValueDto>> GetTickerHistory(SecurityDTO security, DateOnly from, DateOnly to)
        {
            var httpClient = httpClientFactory.CreateClient();

            string query = security.TypeId == SecurityTypeConstants.PreciousMetal ?
                MoexUrlFactory.GetCurrencyHistoricalQuery(security.Ticker, from, to):
                MoexUrlFactory.GetHistoricalQuery(security.Ticker, from, to);

            return await GetHistory(query, httpClient);
        }

        public async Task<IEnumerable<MarketDataRow>> GetExtendedValuesByTickers(IEnumerable<SecurityDTO> securities)
        {
            var httpClient = httpClientFactory.CreateClient();

            var (baseSecurities, currencySecurities) = SplitTickersByType(securities);

            var result = new List<MarketDataRow>();

            if (baseSecurities.Count > 0)
            {
                var query = MoexUrlFactory.GetFullSecuritiesQuery(baseSecurities);
                result.AddRange(await FetchAndApplySecuritiesAsync(httpClient, query));
            }
           
            if (currencySecurities.Count > 0)
            {
                var query = MoexUrlFactory.GetFullCurrencySecuritiesQuery(currencySecurities);
                result.AddRange(await FetchAndApplySecuritiesAsync(httpClient, query));
            }

            return result;
        }

        public async Task<IEnumerable<MarketDataRow>> GetValuesByTickers(IEnumerable<SecurityDTO> securities)
        {
            var httpClient = httpClientFactory.CreateClient();

            var (baseSecurities, currencySecurities) = SplitTickersByType(securities);

            var dataRows = new List<MarketDataRow>();

            if (baseSecurities.Count > 0)
            {
                var baseSecuritiesQuery = MoexUrlFactory.GetBaseSecuritiesQuery(baseSecurities);
                dataRows.AddRange(await FetchMarketDataRowsAsync(httpClient, baseSecuritiesQuery));
            }

            if (currencySecurities.Count > 0)
            {
                var currencySecuritiesQuery = MoexUrlFactory.GetBaseCurrencySecuritiesQuery(currencySecurities);
                dataRows.AddRange(await FetchMarketDataRowsAsync(httpClient, currencySecuritiesQuery));
            }

            return dataRows;
        }

        private static async Task<IEnumerable<MarketDataRow>> FetchMarketDataRowsAsync(HttpClient httpClient, string query)
        {
            var tickersData = await FetchTickersDataAsync(httpClient, query);

            return ParseMarketDataRows(tickersData.MarketData.Columns, tickersData.MarketData);
        }

        private static async Task<MoexResponse> FetchTickersDataAsync(HttpClient httpClient, string query)
        {
            var result = await httpClient.GetAsync(query);
            return await result.Content.ReadFromJsonAsync<MoexResponse>();
        }

        private static async Task<IEnumerable<MarketDataRow>> FetchAndApplySecuritiesAsync(HttpClient httpClient, string query)
        {
            var tickersData = await FetchTickersDataAsync(httpClient, query);

            var marketData = ParseMarketDataRows(tickersData.MarketData.Columns, tickersData.MarketData)
                .ToList();

            return ParseAndApplySecuritiesRows(marketData, tickersData.Securities.Columns, tickersData.Securities);
        }

        private static (HashSet<string> baseSecurities, HashSet<string> currencySecurities) SplitTickersByType(IEnumerable<dynamic> securities)
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

        private static IEnumerable<MarketDataRow> ParseMarketDataRows(IEnumerable<string> columns, DynamicMoexResponseObject marketData)
        {
            var columnsIndexes = GetColumnIndexMapping(columns);

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
                        LastValue = TryGetDecimalValue(row[lastValueIndex]),
                        Date = Convert.ToDateTime(row[dateIndex].ToString()),
                        MarketPrice = TryGetDecimalValue(row[marketPriceIndex]),
                        Open = TryGetDecimalValue(row[openIndex]),
                        Low = TryGetDecimalValue(row[lowIndex], 0),
                        High = TryGetDecimalValue(row[highIndex], 0)
                    }
                )
                .OrderBy(row => GetBoardPriority(row.BoardId));
        }

        private static decimal? TryGetDecimalValue(object value)
        {
            return value != null
                ? Convert.ToDecimal(value.ToString(), CultureInfo.InvariantCulture)
                : null;
        }

        private static decimal TryGetDecimalValue(object value, decimal defaultValue)
        {
            return TryGetDecimalValue(value) ?? defaultValue;
        }

        private static IEnumerable<MarketDataRow> ParseAndApplySecuritiesRows(IEnumerable<MarketDataRow> marketDataRows, 
            IEnumerable<string> columns,
            DynamicMoexResponseObject securities)
        {
            var columnsIndexes = GetColumnIndexMapping(columns);

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

        private static int GetBoardPriority(string boardId) => boardId switch
        {
            "TQBR" => 1,
            "TQTF" => 2,
            "TQPI" => 3,
            "SMAL" => 4,
            "SPEQ" => 5,
            "EQRP" => 6,
            "EQOB" => 7,
            "TQOD" => 8,
            "CETS" => 9,
            _ => 100
        };

        private static async Task<IEnumerable<SecurityHistoryValueDto>> GetHistory(string query, HttpClient httpClient)
        {
            var result = await httpClient.GetAsync(query);
            var tickerHistory = await result.Content.ReadFromJsonAsync<MoexHistoryResponse>();

            var columnsIndexes = GetColumnIndexMapping(tickerHistory.History.Columns);

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

        private static Dictionary<string, int> GetColumnIndexMapping(IEnumerable<string> columns)
        {
            var columnsIndexes = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);

            int i = 0;
            foreach (var col in columns)
            {
                columnsIndexes[col] = i++;
            }

            return columnsIndexes;
        }

        public async Task<IEnumerable<SecurityCandleDto>> GetCandles(SecurityDTO security, DateOnly from, DateOnly to, int interval = 24)
        {
            var httpClient = httpClientFactory.CreateClient();

            string query = security.TypeId == SecurityTypeConstants.PreciousMetal
                ? MoexUrlFactory.GetCurrencyCandlesQuery(security.Ticker, from, to, interval)
                : MoexUrlFactory.GetCandlesQuery(security.Ticker, from, to, interval);

            return await GetCandlesAsync(query, httpClient);
        }

        private static decimal GetDecimalValue(object[] row, int index, decimal defaultValue = 0m)
        {
            return index >= 0 && index < row.Length && row[index] != null
                ? TryGetDecimalValue(row[index], defaultValue)
                : defaultValue;
        }

        private static DateTime GetDateTimeValue(object[] row, int index, DateTime defaultValue = default)
        {
            return index >= 0 && index < row.Length && row[index] != null
                ? Convert.ToDateTime(row[index].ToString(), CultureInfo.InvariantCulture)
                : defaultValue;
        }

        private static async Task<IEnumerable<SecurityCandleDto>> GetCandlesAsync(string query, HttpClient httpClient)
        {
            var result = await httpClient.GetAsync(query);
            var response = await result.Content.ReadFromJsonAsync<MoexCandlesResponse>();

            if (response?.Candles?.Columns == null || response.Candles.Data == null)
            {
                return [];
            }

            var columnsIndexes = GetColumnIndexMapping(response.Candles.Columns);

            int openIdx = columnsIndexes.GetValueOrDefault("open", -1);
            int closeIdx = columnsIndexes.GetValueOrDefault("close", -1);
            int highIdx = columnsIndexes.GetValueOrDefault("high", -1);
            int lowIdx = columnsIndexes.GetValueOrDefault("low", -1);
            int valueIdx = columnsIndexes.GetValueOrDefault("value", -1);
            int volumeIdx = columnsIndexes.GetValueOrDefault("volume", -1);
            int beginIdx = columnsIndexes.GetValueOrDefault("begin", -1);
            int endIdx = columnsIndexes.GetValueOrDefault("end", -1);

            var candles = new List<SecurityCandleDto>();

            foreach (var row in response.Candles.Data)
            {
                if (row == null) continue;

                var candle = new SecurityCandleDto
                {
                    Open = GetDecimalValue(row, openIdx),
                    Close = GetDecimalValue(row, closeIdx),
                    High = GetDecimalValue(row, highIdx),
                    Low = GetDecimalValue(row, lowIdx),
                    Value = GetDecimalValue(row, valueIdx),
                    Volume = GetDecimalValue(row, volumeIdx),
                    Begin = GetDateTimeValue(row, beginIdx),
                    End = GetDateTimeValue(row, endIdx)
                };

                candles.Add(candle);
            }

            return candles;
        }
    }
}
