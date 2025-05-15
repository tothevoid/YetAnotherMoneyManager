using MoneyManager.Shared.Entities;
using MoneyManager.WebApi.Models.Accounts;
using System;

namespace MoneyManager.WebApi.Models.Transactions
{
    public class TransactionModel: BaseEntity
    {
        public string Name {get;set;}

        public DateOnly Date {get;set;}

        public decimal MoneyQuantity {get;set;}

        public AccountModel Account {get;set;}

        public Guid AccountId { get; set; }

        public decimal Cashback { get; set; }

        public bool IsSystem { get; set; }

        public TransactionTypeModel TransactionType { get; set; }

        public Guid TransactionTypeId { get; set; }

    }
}