using System.Collections.Generic;

namespace MoneyManager.Application.DTO.Deposits
{
    public class PeriodPaymentDto
    {
        public string Period { get; set; }

        public IEnumerable<DepositPaymentDto> Payments { get; set; }
    }
}
