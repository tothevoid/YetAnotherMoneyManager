using System.Collections.Generic;

namespace Audex.Application.DTO.Deposits
{
    public class PeriodPaymentDto
    {
        public string Period { get; set; }

        public decimal TotalValue { get; set; }

        public IEnumerable<DepositPaymentDto> Payments { get; set; }
    }
}
