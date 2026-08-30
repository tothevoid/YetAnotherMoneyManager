using System;
using System.Collections.Generic;

namespace Audex.Application.DTO.Deposits
{
    public class PeriodPaymentDto
    {
        public DateOnly Period { get; set; }

        public decimal TotalValue { get; set; }

        public IEnumerable<DepositPaymentDto> Payments { get; set; }
    }
}
