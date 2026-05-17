using System;
using System.Collections.Generic;
using System.Linq;
using MoneyManager.Application.Integrations.Stock.Moex.Builders;

namespace MoneyManager.Application.Integrations.Stock.Moex
{
    public static class MoexUrlFactory
    {
        public static string GetBaseSecuritiesQuery(IEnumerable<string> tickers)
        {
            return GetCommonSecuritiesBuilder(tickers)
                .Build();
        }

        public static string GetBaseCurrencySecuritiesQuery(IEnumerable<string> tickers)
        {
            return GetCommonCurrencySecuritiesBuilder(tickers)
                .Build();
        }

        public static string GetFullSecuritiesQuery(IEnumerable<string> tickers)
        {
            return GetCommonSecuritiesBuilder(tickers)
                .IncludeSecurities()
                .Build();
        }

        public static string GetFullCurrencySecuritiesQuery(IEnumerable<string> tickers)
        {
            return GetCommonCurrencySecuritiesBuilder(tickers)
                .IncludeSecurities()
                .Build();
        }

        private static MoexSecuritiesUrlBuilder GetCommonCurrencySecuritiesBuilder(IEnumerable<string> tickers)
        {
            if (tickers == null || !tickers.Any())
            {
                throw new ArgumentException(nameof(tickers));
            }

            return new MoexCurrencySecuritiesUrlBuilder()
                .IncludeMarket()
                .AddTickers(tickers);
        }

        private static MoexSecuritiesUrlBuilder GetCommonSecuritiesBuilder(IEnumerable<string> tickers)
        {
            if (tickers == null || !tickers.Any())
            {
                throw new ArgumentException(nameof(tickers));
            }

            return new MoexSecuritiesUrlBuilder()
                .IncludeMarket()
                .AddTickers(tickers);
        }

        public static string GetHistoricalQuery(string ticker, DateOnly from, DateOnly to)
        {
            var builder = new MoexHistoryUrlBuilder(ticker);

            return builder
                .IncludeHistory()
                .AddRange(from, to)
                .Build();
        }

        public static string GetCurrencyHistoricalQuery(string ticker, DateOnly from, DateOnly to)
        {
            var builder = new MoexCurrencyHistoryUrlBuilder(ticker);

            return builder
                .IncludeHistory()
                .AddRange(from, to)
                .Build();
        }
    }
}
