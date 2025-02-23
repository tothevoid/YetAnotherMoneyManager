using MoneyManager.Common;
using System;

namespace MoneyManager.DAL.Entities
{
    public class Transaction : BaseEntity
    {
        public string Name { get; set; }

        public DateTime Date { get; set; }

        public double MoneyQuantity { get; set; }

        public Fund FundSource { get; set; }

        public Guid FundSourceId { get; set; }

        //public TransactionType TransactionType { get; set; }

        public string TransactionType { get; set; }

        public Guid TransactionTypeId { get; set; }

        public Transaction AssignFund(Fund fund)
        {
            FundSource = fund;
            return this;
        }

        //public Transaction AssignType(TransactionType transactionType)
        //{
        //    TransactionType = transactionType;
        //    return this;
        //}
    }
}
