using MoneyManager.Shared.Entities;
using System;
using MoneyManager.Infrastructure.Entities.Currencies;

namespace MoneyManager.Infrastructure.Entities.Deposits
{
    public class Deposit : BaseEntity
    {
        public string Name { get; set; }

        public DateOnly From { get; set; }

        public DateOnly To { get; set; }

        public decimal Percentage { get; set; }

        public decimal InitialAmount { get; set; }

        public decimal EstimatedEarn { get; set; }

        public Guid CurrencyId { get; set; }

        public Currency Currency { get; set; }
    }
}