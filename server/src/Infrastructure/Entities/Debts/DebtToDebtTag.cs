using System;
using Audex.Shared.Entities;

namespace Audex.Infrastructure.Entities.Debts
{
    public class DebtToDebtTag : BaseEntity
    {
        public Guid DebtId { get; set; }

        public Debt Debt { get; set; } = null!;

        public Guid DebtTagId { get; set; }

        public DebtTag DebtTag { get; set; } = null!;
    }
}
