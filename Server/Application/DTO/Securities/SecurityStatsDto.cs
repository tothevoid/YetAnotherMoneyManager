using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyManager.Application.DTO.Securities
{
    public class SecurityStatsDto
    {
        public decimal TransactionsMin { get; set; }

        public decimal TransactionsMax { get; set; }

        public decimal TransactionsAvg { get; set; }

        public int HasOnBrokerAccounts { get; set; }

        public decimal DividendsIncome { get; set; }
    }
}
