using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyManager.Application.DTO.Brokers
{
    public class BrokerAccountMonthTransferDto
    {
        public int MonthIndex { get; set; }

        public bool Income { get; set; }

        public decimal Value { get; set; }
    }
}
