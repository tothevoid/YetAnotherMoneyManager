using System;
using System.Collections.Generic;

namespace Audex.WebApi.Models.Deposits.Charts
{
    public class PeriodPayment
    {
        public DateOnly Period { get; set; }

        public decimal TotalValue { get; set; }

        public IEnumerable<DepositPayment> Payments { get; set; }
    }
}
