using System;
using System.Collections.Generic;

namespace Audex.WebApi.Models.Deposits.Charts
{
    public class DepositMonthSummary
    {
        public decimal TotalEarnings { get; set; }

        public decimal AverageMonthly { get; set; }

        public DateOnly? PeakMonthPeriod { get; set; }

        public decimal PeakMonthValue { get; set; }

        public int MonthsCount { get; set; }

        public IEnumerable<DepositSummaryItem> DepositTotals { get; set; }

        public IEnumerable<PeriodPayment> Payments { get; set; }
    }
}
