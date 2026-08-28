namespace MoneyManager.Application.Integrations.Stock.Moex.Builders
{
    public class MoexCurrencySecuritiesUrlBuilder : MoexSecuritiesUrlBuilder
    {
        protected override string GetUrl()
        {
            return $"{BaseUrl}/engines/currency/markets/selt/securities.json";
        }
    }
}
