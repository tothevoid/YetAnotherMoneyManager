using System.Collections.Generic;

namespace MoneyManager.WebApi.Models.Deposit.Charts
{
    public class DepositMonthSummary
    {
        public IEnumerable<string> Deposits { get; set; }

        public IEnumerable<PeriodPayment> Payments { get; set; }
    }
}
