using System.Collections.Generic;

namespace Audex.Application.DTO.Deposits
{
    public class DepositMonthSummaryDto
    {
        public IEnumerable<PeriodPaymentDto> Payments { get; set; }
    }
}
