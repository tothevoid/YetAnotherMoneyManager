using System;

namespace MoneyManager.Application.Integrations.Stock.Moex.Builders
{
    public class MoexCurrencyCandlesUrlBuilder : MoexCandlesUrlBuilder
    {
        public MoexCurrencyCandlesUrlBuilder(string ticker) : base(ticker)
        {
        }

        protected override string GetUrl()
        {
            return $"{BaseUrl}/engines/currency/markets/selt/securities/{Ticker}/candles.json";
        }
    }
}
