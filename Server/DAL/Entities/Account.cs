using MoneyManager.Common;
using System;

namespace MoneyManager.DAL.Entities
{
    public class Account : BaseEntity
    {
        public string Name { get; set; }

        public decimal Balance { get; set; }

        public Currency Currency { get; set; }

        public AccountType AccountType { get; set; }

        public Guid CurrencyId { get; set; }

        public Guid AccountTypeId { get; set; }

        public DateOnly CreatedOn { get; set; }

        public bool Active { get; set; }

        public Account AssignReferences(Currency currency, AccountType accountType)
        {
            Currency = currency;
            AccountType = accountType;
            return this;
        }
    }
}
