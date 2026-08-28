using System;

namespace MoneyManager.Application.Integrations.Stock.Moex.Builders
{
    public class MoexHistoryUrlBuilder : BaseMoexUrlBuilder
    {
        protected virtual string Ticker { get; }

        public MoexHistoryUrlBuilder(string ticker)
        {
            Ticker = ticker;
        }

        public MoexHistoryUrlBuilder IncludeHistory()
        {
            OutputFilters.Add("history");
            AdditionalParameters.Add("history.columns=TRADEDATE,CLOSE");
            return this;
        }

        protected override string GetUrl()
        {
            return $"{BaseUrl}/history/engines/stock/markets/shares/securities/{Ticker}.json";
        }
    }
}
