namespace MoneyManager.Application.Integrations.Stock.Moex.Builders
{
    public class MoexHistoryUrlBuilder: BaseMoexUrlBuilder
    {
        public MoexHistoryUrlBuilder(string ticker)
        {
            Url = ApplyTickerToUrl(ticker);
        }
       
        public MoexHistoryUrlBuilder IncludeHistory()
        {
            OutputFilters.Add("history");
            AdditionalParameters.Add("history.columns=TRADEDATE,CLOSE");
            return this;
        }

        private string ApplyTickerToUrl(string ticker)
        {
            return $"{Url}/history/engines/stock/markets/shares/securities/{ticker}.json";
        }
    }
}
