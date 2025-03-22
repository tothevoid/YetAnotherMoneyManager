using MoneyManager.Common;
using System;

namespace MoneyManager.DAL.Entities
{
    public class Account : BaseEntity
    {
        public string Name { get; set; }

        public double Balance { get; set; }

        public Currency Currency { get; set; }

        public Guid CurrencyId { get; set; }

        public DateOnly CreatedOn { get; set; }

        public bool Active { get; set; }

        public Account AssignCurrency(Currency currency)
        {
            Currency = currency;
            return this;
        }
    }
}
