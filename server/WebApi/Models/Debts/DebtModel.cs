using System;
using MoneyManager.Shared.Entities;
using MoneyManager.WebApi.Models.Currencies;

namespace MoneyManager.WebApi.Models.Debts
{
    public class DebtModel: BaseEntity
    {
        public string Name { get; set; }

        public CurrencyModel Currency { get; set; }

        public Guid CurrencyId { get; set; }

        public decimal Amount { get; set; }

        public DateOnly Date { get; set; }
    }
}
