using MoneyManager.Common;
using System;

namespace MoneyManager.DAL.Entities
{
    public class Transaction : BaseEntity
    {
        public string Name { get; set; }

        public DateTime Date { get; set; }

        public double MoneyQuantity { get; set; }

        public Account Account { get; set; }

        public Guid AccountId { get; set; }

        //public TransactionType TransactionType { get; set; }

        public string TransactionType { get; set; }

        public Guid TransactionTypeId { get; set; }

        public Transaction AssignAccount(Account account)
        {
            Account = account;
            return this;
        }

        //public Transaction AssignType(TransactionType transactionType)
        //{
        //    TransactionType = transactionType;
        //    return this;
        //}
    }
}
