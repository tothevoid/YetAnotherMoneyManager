using System;
using System.Collections.Generic;
using BLL.DTO;
using MoneyManager.Common;

namespace MoneyManager.Model.Charts.Deposit
{
    public class DepositMonthSummary
    {
        public IEnumerable<string> Deposits { get; set; }

        public IEnumerable<PeriodPayment> Payments { get; set; }
    }
}
