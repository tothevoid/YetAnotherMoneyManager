using System.Collections.Generic;

namespace MoneyManager.Application.DTO.Deposits
{
    public class DepositMonthSummaryDTO
    {
        public IEnumerable<PeriodPaymentDTO> Payments { get; set; }
    }
}
