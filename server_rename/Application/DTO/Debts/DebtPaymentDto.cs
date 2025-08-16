using System;
using MoneyManager.Application.DTO.Accounts;
using MoneyManager.Application.DTO.Transactions;
using MoneyManager.Shared.Entities;

namespace MoneyManager.Application.DTO.Debts
{
    public class DebtPaymentDto: BaseEntity
    {
        public DebtDto Debt { get; set; }

        public Guid DebtId { get; set; }

        public AccountDTO TargetAccount { get; set; }

        public Guid TargetAccountId { get; set; }

        public DateOnly Date { get; set; }

        public decimal Amount { get; set; }
    }
}
