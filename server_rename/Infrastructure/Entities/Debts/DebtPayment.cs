using System;
using MoneyManager.Infrastructure.Entities.Accounts;
using MoneyManager.Infrastructure.Entities.Transactions;
using MoneyManager.Shared.Entities;

namespace MoneyManager.Infrastructure.Entities.Debts
{
    public class DebtPayment: BaseEntity
    {
        public Debt Debt { get; set; }

        public Guid DebtId { get; set; }

        public Account TargetAccount { get; set; }

        public Guid TargetAccountId { get; set; }

        public DateOnly Date { get; set; }

        public decimal Amount { get; set; }
    }
}
