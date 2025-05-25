using System;
using MoneyManager.Infrastructure.Entities.Currencies;
using MoneyManager.Shared.Entities;

namespace MoneyManager.Infrastructure.Entities.Debts
{
    public class Debt: BaseEntity
    {
        public string Name { get; set; }

        public Currency Currency { get; set; }

        public Guid CurrencyId { get; set; }

        public decimal Amount { get; set; }

        public DateOnly Date { get; set; }

        public DateOnly PaidOn { get; set; }

    }
}
