using System.Collections.Generic;

namespace MoneyManager.Application.DTO.Deposits
{
    public class DepositMonthSummaryDTO
    {
        public IEnumerable<string> Deposits { get; set; }

        public IEnumerable<PeriodPaymentDTO> Payments { get; set; }
    }
}
