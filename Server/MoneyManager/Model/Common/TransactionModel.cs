using MoneyManager.Common;
using System;

namespace MoneyManager.WEB.Model
{
    public class TransactionModel: BaseEntity
    {
        public string Name {get;set;}

        public DateTime Date {get;set;}

        public double MoneyQuantity {get;set;}

        public AccountModel Account {get;set;}

        public Guid AccountId { get; set; }

        public string TransactionType { get; set; }

        //public TransactionTypeModel TransactionType { get; set; }

        //public Guid TransactionTypeId { get; set; }

    }
}