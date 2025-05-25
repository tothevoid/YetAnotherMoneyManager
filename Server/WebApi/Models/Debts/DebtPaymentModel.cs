using System;
using MoneyManager.Application.DTO.Accounts;
using MoneyManager.Shared.Entities;
using MoneyManager.WebApi.Models.Accounts;
using MoneyManager.WebApi.Models.Transactions;

namespace MoneyManager.WebApi.Models.Debts
{
    public class DebtPaymentModel: BaseEntity
    {
        public DebtModel Debt { get; set; }

        public Guid DebtId { get; set; }

        public AccountModel TargetAccount { get; set; }

        public Guid TargetAccountId { get; set; }

        public TransactionModel Transaction { get; set; }

        public Guid TransactionId { get; set; }

        public DateOnly Date { get; set; }

        public decimal Amount { get; set; }
    }
}
