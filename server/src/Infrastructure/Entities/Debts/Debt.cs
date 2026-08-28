using System;
using Audex.Infrastructure.Entities.Brokers;
using System.Collections.Generic;
using Audex.Infrastructure.Entities.Currencies;
using Audex.Shared.Entities;

namespace Audex.Infrastructure.Entities.Debts
{
    public class Debt: BaseEntity
    {
        public string Name { get; set; }

        public Currency Currency { get; set; }

        public Guid CurrencyId { get; set; }

        public decimal Amount { get; set; }

        public DateOnly Date { get; set; }

        public ICollection<DebtPayment> DebtPayments { get; set; }

        public ICollection<DebtToDebtTag> DebtTags { get; set; } = new List<DebtToDebtTag>();
    }
}
