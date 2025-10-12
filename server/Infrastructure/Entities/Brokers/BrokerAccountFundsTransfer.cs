using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MoneyManager.Infrastructure.Entities.Accounts;
using MoneyManager.Shared.Entities;

namespace MoneyManager.Infrastructure.Entities.Brokers
{
    public class BrokerAccountFundsTransfer: BaseEntity
    {
        public DateTime Date { get; set; }

        public decimal Amount { get; set; }

        public Account Account { get; set; }

        public Guid AccountId { get; set; }

        public BrokerAccount BrokerAccount { get; set; }

        public Guid BrokerAccountId { get; set; }

        public bool Income { get; set; }
    }
}
