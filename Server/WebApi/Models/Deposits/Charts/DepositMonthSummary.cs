﻿using System.Collections.Generic;

namespace MoneyManager.WebApi.Models.Deposits.Charts
{
    public class DepositMonthSummary
    {
        public IEnumerable<PeriodPayment> Payments { get; set; }
    }
}
