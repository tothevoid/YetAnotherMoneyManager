using System;
using System.Collections.Generic;
using BLL.DTO;
using MoneyManager.Common;

namespace MoneyManager.Model.Server
{
    public class DepositMonthSummaryDTO
    {
        public IEnumerable<string> Deposits { get; set; }

        public IEnumerable<PeriodPaymentDTO> Payments { get; set; }
    }
}
