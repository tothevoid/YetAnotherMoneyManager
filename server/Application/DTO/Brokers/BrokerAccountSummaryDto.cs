using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyManager.Application.DTO.Brokers
{
    public class BrokerAccountSummaryDto
    {
        public decimal TotalIncome { get; set; }
        public decimal TotalDeposit { get; set; }
        public decimal TotalWithdraw { get; set; }
        public List<DailyStatDto> DailyStats { get; set; }
    }

    public class DailyStatDto
    {
        public DailyStatDto Security { get; set; }
        public decimal FirstPrice { get; set; }
        public decimal LastPrice { get; set; }
        public decimal MinPrice { get; set; }
        public decimal MaxPrice { get; set; }
    }
}
