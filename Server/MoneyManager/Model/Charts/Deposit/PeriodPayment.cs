using MoneyManager.Model.Server;
using System.Collections.Generic;

namespace MoneyManager.Model.Charts.Deposit
{
    public class PeriodPayment
    {
        public string Period { get; set; }

        public IEnumerable<DepositPayment> Payments { get; set; }
    }
}
