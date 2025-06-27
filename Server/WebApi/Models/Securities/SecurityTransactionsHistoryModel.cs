using System.Collections.Generic;
using System;

namespace MoneyManager.WebApi.Models.Securities
{
    public class SecurityTransactionsHistoryModel
    {
        public DateTime Date { get; set; }

        public decimal ValueWithPayments { get; set; }

        public decimal ValueWithoutPayments { get; set; }
    }
}
