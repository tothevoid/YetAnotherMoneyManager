using System;
using System.Collections.Generic;
using MoneyManager.Application.DTO.Securities;

namespace MoneyManager.Application.DTO.Brokers
{
    public class BrokerAccountDailyStatsDto
    {
        public DateTime FetchDate { get; set; }

        public decimal StartPortfolioValue { get; set; }

        public decimal CurrentPortfolioValue { get; set; }

        public IEnumerable<BrokerAccountDailySecurityStatsDto> BrokerAccountDailySecurityStats { get; set; }
    }

    public class BrokerAccountDailySecurityStatsDto
    {
        public SecurityDTO Security { get; set; }

        public decimal StartPrice { get; set; }

        public decimal CurrentPrice { get; set; }

        public decimal MinPrice { get; set; }

        public decimal MaxPrice { get; set; }

        public decimal PreviousDayClosePrice { get; set; }
    }
}
