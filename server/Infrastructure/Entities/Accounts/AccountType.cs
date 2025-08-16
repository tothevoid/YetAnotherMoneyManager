using MoneyManager.Shared.Entities;
using System.Collections.Generic;

namespace MoneyManager.Infrastructure.Entities.Accounts
{
    public class AccountType : BaseEntity
    {
        public string Name { get; set; }

        public bool Active { get; set; }

        public ICollection<Account> Accounts { get; set; }
    }
}