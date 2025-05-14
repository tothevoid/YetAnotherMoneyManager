using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyManager.Application.Integrations.Stock.Moex
{
    public class MoexUrlBuilder
    {
        private readonly string _baseUrl = "https://iss.moex.com/iss";

        private readonly List<string> _additionalParameters = new();

        public MoexUrlBuilder RemoveMeta()
        {
            _additionalParameters.Add("iss.meta=off");
            return this;
        }

        public MoexUrlBuilder IncludeOnlyMarketData()
        {
            _additionalParameters.Add("iss.only=marketdata");
            return this;
        }

        public MoexUrlBuilder IncludeOnlyHistoryData()
        {
            _additionalParameters.Add("iss.only=history");
            return this;
        }

        public MoexUrlBuilder SelectMarketColumns()
        {
            _additionalParameters.Add("marketdata.columns=SECID,LAST,SYSTIME,MARKETPRICE");
            return this;
        }

        public MoexUrlBuilder SelectHistoryColumns()
        {
            _additionalParameters.Add("history.columns=TRADEDATE,CLOSE");
            return this;
        }

        private void AddTickers(IEnumerable<string> tickers)
        {
            _additionalParameters.Add($"securities={string.Join(',', tickers)}");
        }

        private void AddRange(DateOnly from, DateOnly to)
        {
            var pattern = "yyyy-MM-dd";
            _additionalParameters.Add($"from={from.ToString(pattern)}&till={to.ToString(pattern)}");
        }

        public string GetHistoricalQuery(string ticker, DateOnly from, DateOnly to)
        {
            IncludeOnlyHistoryData();
            SelectHistoryColumns();
            AddRange(from, to);

            var url = $"{_baseUrl}/history/engines/stock/markets/shares/securities/{ticker}.json";

            return GenerateUrl(url);
        }

        public string BuildSecuritiesQuery(IEnumerable<string> tickers)
        {
            if (tickers == null || !tickers.Any())
            {
                throw new ArgumentException(nameof(tickers));
            }

            RemoveMeta();
            IncludeOnlyMarketData();
            SelectMarketColumns();
            AddTickers(tickers);

            var url = $"{_baseUrl}/engines/stock/markets/shares/securities.json";
            return GenerateUrl(url);
        }

        private string GenerateUrl(string url)
        {
            if (_additionalParameters.Any())
            {
                var combinedParameters = string.Join("&", _additionalParameters);
                return $"{url}?{combinedParameters}";
            }

            return url;
        }
    }
}
