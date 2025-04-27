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

        public MoexUrlBuilder SelectMarketColumns()
        {
            _additionalParameters.Add("marketdata.columns=SECID,LAST,SYSTIME");
            return this;
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

            var url = $"{_baseUrl}/iss/engines/stock/markets/shares/securities.json";
            if (_additionalParameters.Any())
            {
                var combinedParameters = string.Join("&", _additionalParameters);
                return $"{url}?{combinedParameters}";
            }

            return url;
        }
    }
}
