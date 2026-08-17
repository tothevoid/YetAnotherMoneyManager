using System.Collections.Generic;

namespace MoneyManager.Application.DTO.Deposits
{
    public class DepositMonthSummaryDto
    {
        public IEnumerable<PeriodPaymentDto> Payments { get; set; }
    }
}
