using MoneyManager.Infrastructure.Entities.Accounts;
using MoneyManager.Shared.Entities;
using System;
using System.Collections.Generic;
using MoneyManager.Infrastructure.Entities.Debts;

namespace MoneyManager.Infrastructure.Entities.Transactions
{
    public class Transaction : BaseEntity
    {
        public string Name { get; set; }

        public DateOnly Date { get; set; }

        public decimal Amount { get; set; }

        public Account Account { get; set; }

        public Guid AccountId { get; set; }

        public TransactionType TransactionType { get; set; }

        public Guid TransactionTypeId { get; set; }

        public decimal Cashback { get; set; }

        public bool IsSystem { get; set; }

        public ICollection<DebtPayment> DebtPayments { get; set; }
    }
}
