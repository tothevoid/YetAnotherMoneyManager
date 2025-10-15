using System.Collections.Generic;
using MoneyManager.WebApi.Models.Securities;

namespace MoneyManager.WebApi.Models.Brokers
{
    public class BrokerAccountSummaryModel
    {
        public decimal TotalIncome { get; set; }
        public decimal TotalDeposit { get; set; }
        public decimal TotalWithdraw { get; set; }
        public List<DailyStatModel> DailyStats { get; set; }
    }

    public class DailyStatModel
    {
        public SecurityModel Security { get; set; }
        public decimal FirstPrice { get; set; }
        public decimal LastPrice { get; set; }
        public decimal MinPrice { get; set; }
        public decimal MaxPrice { get; set; }
    }
}
