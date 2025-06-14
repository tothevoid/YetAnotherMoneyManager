using MoneyManager.Application.DTO.Accounts;
using MoneyManager.Shared.Entities;
using System;

namespace MoneyManager.Application.DTO.Transactions
{
    public class TransactionDTO : BaseEntity
    {
        public string Name { get; set; }

        public DateOnly Date { get; set; }

        public decimal Amount { get; set; }

        public AccountDTO Account { get; set; }

        public Guid AccountId { get; set; }

        public TransactionTypeDTO TransactionType { get; set; }

        public Guid TransactionTypeId { get;set; }

        public decimal Cashback { get; set; }

        public bool IsSystem { get; set; }
    }
}
