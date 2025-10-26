using MoneyManager.Application.DTO.Securities;
using System.Collections.Generic;

namespace MoneyManager.WebApi.Models.Brokers
{
    public class BrokerAccountSummaryModel
    {
        public BrokerAccountTransfersStatsModel TransferStats { get; set; }

        public BrokerAccountStatsModel BrokerAccountStats { get; set; }
    }

    public class BrokerAccountStatsModel
    {
        public IEnumerable<BrokerAccountSecurityStatsModel> SecurityStats { get; set; }

        public decimal InvestedValue { get; set; }

        public decimal CurrentValue { get; set; }

        public decimal TotalDividendsValue { get; set; }
    }

    public class BrokerAccountSecurityStatsModel
    {
        public SecurityDTO Security { get; set; }

        public decimal FirstPrice { get; set; }

        public decimal LastPrice { get; set; }

        public decimal MinPrice { get; set; }

        public decimal MaxPrice { get; set; }
    }

    public class BrokerAccountTransfersStatsModel
    {
        public decimal TotalDeposited { get; set; }

        public decimal TotalWithdrawn { get; set; }
    }
}
