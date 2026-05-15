using System.Collections.Generic;

namespace MoneyManager.Application.Integrations.Stock.Moex.Builders
{
    public class MoexCurrencyUrlBuilder: BaseMoexUrlBuilder
    {
        public MoexCurrencyUrlBuilder()
        {
            Url = $"{Url}/engines/currency/markets/selt/securities.json";
        }

        public MoexCurrencyUrlBuilder IncludeMarket()
        {
            OutputFilters.Add("marketdata");
            AdditionalParameters.Add("marketdata.columns=BOARDID,OPEN,SECID,LAST,SYSTIME,MARKETPRICE,LOW,HIGH");

            return this;
        }

        public MoexCurrencyUrlBuilder IncludeSecurities()
        {
            OutputFilters.Add("securities");
            AdditionalParameters.Add("securities.columns=SECID,BOARDID,PREVPRICE");

            return this;
        }

        public MoexCurrencyUrlBuilder AddTickers(IEnumerable<string> tickers)
        {
            AdditionalParameters.Add($"securities={string.Join(',', tickers)}");
            return this;
        }

    }
}
