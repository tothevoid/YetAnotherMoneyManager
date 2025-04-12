using System.Collections.Generic;

namespace MoneyManager.WebApi.Models.Deposit.Charts
{
    public class PeriodPayment
    {
        public string Period { get; set; }

        public IEnumerable<DepositPayment> Payments { get; set; }
    }
}
