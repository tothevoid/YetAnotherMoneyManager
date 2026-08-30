using System.Collections.Generic;

namespace Audex.Application.DTO.Deposits
{
    public class DepositMonthSummaryDto
    {
        public decimal TotalEarnings { get; set; }

        public decimal AverageMonthly { get; set; }

        public string PeakMonthPeriod { get; set; }

        public decimal PeakMonthValue { get; set; }

        public int MonthsCount { get; set; }

        public IEnumerable<DepositSummaryItemDto> DepositTotals { get; set; }

        public IEnumerable<PeriodPaymentDto> Payments { get; set; }
    }
}
