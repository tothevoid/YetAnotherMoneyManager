using System.Collections.Generic;

namespace MoneyManager.Application.Integrations.Stock.Moex.Model
{
    public class FullSecurityData
    {
        public IEnumerable<MarketDataRow> MarketData { get; set; }

        public IEnumerable<SecurityRow> Security { get; set; }
    }
}
