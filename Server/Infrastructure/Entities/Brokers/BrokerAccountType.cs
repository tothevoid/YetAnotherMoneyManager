using MoneyManager.Infrastructure.Entities.Accounts;
using MoneyManager.Shared.Entities;
using System.Collections.Generic;

namespace MoneyManager.Infrastructure.Entities.Brokers
{
    public class BrokerAccountType: BaseEntity
    {
        public string Name { get; set; }

        public ICollection<BrokerAccount> BrokerAccounts { get; set; }
    }
}
