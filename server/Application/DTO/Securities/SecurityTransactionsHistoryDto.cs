using System;
using System.Collections.Generic;

namespace MoneyManager.Application.DTO.Securities
{
    public class SecurityTransactionsHistoryDto
    {
        public DateTime Date { get; set; }

        public decimal ValueWithPayments { get; set; }

        public decimal ValueWithoutPayments { get; set; }
    }
}
