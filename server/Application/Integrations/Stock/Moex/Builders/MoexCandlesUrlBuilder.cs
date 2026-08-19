using System;

namespace MoneyManager.Application.Integrations.Stock.Moex.Builders
{
    public class MoexCandlesUrlBuilder : BaseMoexUrlBuilder
    {
        protected string Ticker { get; }

        public MoexCandlesUrlBuilder(string ticker)
        {
            Ticker = ticker;
        }

        public MoexCandlesUrlBuilder IncludeCandles()
        {
            OutputFilters.Add("candles");
            return this;
        }

        public MoexCandlesUrlBuilder AddInterval(int interval = 24)
        {
            AdditionalParameters.Add($"interval={interval}");
            return this;
        }

        public MoexCandlesUrlBuilder AddStart(int start = 0)
        {
            if (start > 0)
            {
                AdditionalParameters.Add($"start={start}");
            }
            return this;
        }

        protected override string GetUrl()
        {
            return $"{BaseUrl}/engines/stock/markets/shares/securities/{Ticker}/candles.json";
        }
    }
}
