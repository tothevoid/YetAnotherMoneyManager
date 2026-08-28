using System.Collections.Generic;

namespace Audex.Application.Integrations.Stock.Moex.Model
{
    public class FullSecurityData
    {
        public IEnumerable<MarketDataRow> MarketData { get; set; }

        public IEnumerable<SecurityRow> Security { get; set; }
    }
}
