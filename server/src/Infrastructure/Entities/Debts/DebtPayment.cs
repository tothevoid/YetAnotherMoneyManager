using System;
using Audex.Infrastructure.Entities.Accounts;
using Audex.Shared.Entities;

namespace Audex.Infrastructure.Entities.Debts
{
    public class DebtPayment: BaseEntity
    {
        public Debt Debt { get; set; }

        public Guid DebtId { get; set; }

        public Account TargetAccount { get; set; }

        public Guid TargetAccountId { get; set; }

        public DateOnly Date { get; set; }

        public decimal Amount { get; set; }

        public bool IsPercentagePayment { get; set; }
    }
}
