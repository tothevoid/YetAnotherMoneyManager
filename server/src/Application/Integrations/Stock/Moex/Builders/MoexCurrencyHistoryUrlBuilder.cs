namespace MoneyManager.Application.Integrations.Stock.Moex.Builders
{
    public class MoexCurrencyHistoryUrlBuilder: MoexHistoryUrlBuilder
    {
        public MoexCurrencyHistoryUrlBuilder(string ticker) : base(ticker)
        {
        }

        protected override string GetUrl()
        {
            return $"{BaseUrl}/history/engines/currency/markets/selt/securities/{Ticker}.json";
        }
    }
}
