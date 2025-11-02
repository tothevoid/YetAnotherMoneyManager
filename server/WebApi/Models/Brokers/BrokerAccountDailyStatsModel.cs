using System;
using System.Collections.Generic;
using MoneyManager.WebApi.Models.Securities;

namespace MoneyManager.WebApi.Models.Brokers
{
    public class BrokerAccountDailyStatsModel
    {
        public DateTime FetchDate { get; set; }

        public decimal StartPortfolioValue { get; set; }

        public decimal CurrentPortfolioValue { get; set; }

        public IEnumerable<BrokerAccountDailySecurityStatsModel> BrokerAccountDailySecurityStats { get; set; }
    }

    public class BrokerAccountDailySecurityStatsModel
    {
        public SecurityModel Security { get; set; }

        public decimal StartPrice { get; set; }

        public decimal CurrentPrice { get; set; }
    }
}
