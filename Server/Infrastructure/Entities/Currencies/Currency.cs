using MoneyManager.Infrastructure.Entities.Accounts;
using MoneyManager.Infrastructure.Entities.Brokers;
using MoneyManager.Shared.Entities;
using System.Collections.Generic;
using MoneyManager.Infrastructure.Entities.Debts;
using MoneyManager.Infrastructure.Entities.Securities;
using MoneyManager.Infrastructure.Entities.User;

namespace MoneyManager.Infrastructure.Entities.Currencies
{
    public class Currency : BaseEntity
    {
        public string Name { get; set; }

        public bool Active { get; set; }

        public decimal Rate { get; set; }

        public ICollection<Account> Accounts { get; set; }

        public ICollection<BrokerAccount> BrokerAccounts { get; set; }

        public ICollection<Debt> Debts { get; set; }

        public ICollection<Security> Securities { get; set; }

        public ICollection<UserProfile> UserProfiles { get; set; }
    }
}