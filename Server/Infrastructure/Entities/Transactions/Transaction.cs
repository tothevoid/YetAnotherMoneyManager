using MoneyManager.Infrastructure.Entities.Accounts;
using MoneyManager.Shared.Entities;
using System;

namespace MoneyManager.Infrastructure.Entities.Transactions
{
    public class Transaction : BaseEntity
    {
        public string Name { get; set; }

        public DateOnly Date { get; set; }

        public decimal MoneyQuantity { get; set; }

        public Account Account { get; set; }

        public Guid AccountId { get; set; }

        public TransactionType TransactionType { get; set; }

        public Guid TransactionTypeId { get; set; }

        public decimal Cashback { get; set; }

        public bool IsSystem { get; set; }
    }
}
