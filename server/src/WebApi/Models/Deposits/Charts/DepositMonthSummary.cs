using System.Collections.Generic;

namespace Audex.WebApi.Models.Deposits.Charts
{
    public class DepositMonthSummary
    {
        public IEnumerable<PeriodPayment> Payments { get; set; }
    }
}
