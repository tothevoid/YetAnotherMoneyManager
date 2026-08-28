using System;
using MoneyManager.Shared.Entities;

namespace MoneyManager.Infrastructure.Entities.Debts
{
    public class DebtToDebtTag : BaseEntity
    {
        public Guid DebtId { get; set; }

        public Debt Debt { get; set; } = null!;

        public Guid DebtTagId { get; set; }

        public DebtTag DebtTag { get; set; } = null!;
    }
}
