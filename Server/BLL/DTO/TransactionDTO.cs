using MoneyManager.Common;
using System;

namespace MoneyManager.BLL.DTO
{
    public class TransactionDTO : BaseEntity
    {
        public string Name { get; set; }

        public DateTime Date { get; set; }

        public decimal MoneyQuantity { get; set; }

        public AccountDTO Account { get; set; }

        public Guid AccountId { get; set; }

        //public TransactionTypeDTO TransactionType { get; set; }
        public string TransactionType { get; set; }

        //public Guid TransactionTypeId { get;set; }

        public decimal Cashback { get; set; }
    }
}
