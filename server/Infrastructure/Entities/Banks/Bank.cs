using MoneyManager.Infrastructure.Entities.Brokers;
using MoneyManager.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MoneyManager.Infrastructure.Entities.Accounts;
using MoneyManager.Infrastructure.Entities.Deposits;

namespace MoneyManager.Infrastructure.Entities.Banks
{
    public class Bank : BaseEntity
    {
        public string Name { get; set; }

        public string IconKey { get; set; }

        public ICollection<BrokerAccount> BrokerAccounts { get; set; }

        public ICollection<Account> Accounts { get; set; }

        public ICollection<Deposit> Deposits { get; set; }
    }
}
