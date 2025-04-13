using System.Collections.Generic;

namespace MoneyManager.Application.DTO.Deposits
{
    public class PeriodPaymentDTO
    {
        public string Period { get; set; }

        public IEnumerable<DepositPaymentDTO> Payments { get; set; }
    }
}
