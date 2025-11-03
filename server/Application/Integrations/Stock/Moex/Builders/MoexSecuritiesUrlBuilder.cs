using System.Collections.Generic;

namespace MoneyManager.Application.Integrations.Stock.Moex.Builders
{
    public class MoexSecuritiesUrlBuilder: BaseMoexUrlBuilder
    {
        public MoexSecuritiesUrlBuilder()
        {
            Url = $"{Url}/engines/stock/markets/shares/securities.json";
        }

        public MoexSecuritiesUrlBuilder IncludeMarket()
        {
            OutputFilters.Add("marketdata");
            AdditionalParameters.Add("marketdata.columns=BOARDID,OPEN,SECID,LAST,SYSTIME,MARKETPRICE,LOW,HIGH");

            return this;
        }

        public MoexSecuritiesUrlBuilder IncludeSecurities()
        {
            OutputFilters.Add("securities");
            AdditionalParameters.Add("securities.columns=SECID,BOARDID,PREVPRICE");

            return this;
        }

        public MoexSecuritiesUrlBuilder AddTickers(IEnumerable<string> tickers)
        {
            AdditionalParameters.Add($"securities={string.Join(',', tickers)}");
            return this;
        }

    }
}
