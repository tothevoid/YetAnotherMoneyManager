using Audex.Infrastructure.Entities.Accounts;
using Audex.Infrastructure.Entities.Brokers;
using Audex.Shared.Entities;
using System.Collections.Generic;
using Audex.Infrastructure.Entities.Debts;
using Audex.Infrastructure.Entities.Deposits;
using Audex.Infrastructure.Entities.Securities;
using Audex.Infrastructure.Entities.User;

namespace Audex.Infrastructure.Entities.Currencies
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

        public ICollection<Deposit> Deposits { get; set; }
    }
}