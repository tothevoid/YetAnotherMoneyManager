using System.Collections.Generic;
using MoneyManager.Application.DTO.Securities;

namespace MoneyManager.Application.DTO.Brokers
{
    public class BrokerAccountSummaryDto
    {
        public BrokerAccountTransfersStatsDto TransferStats { get; set; }

        public BrokerAccountStatsDto BrokerAccountStats { get; set; }
    }

    public class BrokerAccountStatsDto
    {
        public IEnumerable<BrokerAccountSecurityStatsDto> SecurityStats { get; set; }

        public decimal InvestedValue { get; set; }

        public decimal CurrentValue { get; set; }

        public decimal TotalDividendsValue { get; set; }
    }

    public class BrokerAccountSecurityStatsDto
    {
        public SecurityDTO Security { get; set; }

        public decimal FirstPrice { get; set; }

        public decimal LastPrice { get; set; }

        public decimal MinPrice { get; set; }

        public decimal MaxPrice { get; set; }
    }

    public class BrokerAccountTransfersStatsDto
    {
        public decimal TotalDeposited { get; set; }

        public decimal TotalWithdrawn { get; set; }
    }
}
