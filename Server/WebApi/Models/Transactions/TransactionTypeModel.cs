using System;
using MoneyManager.Shared.Entities;

namespace MoneyManager.WebApi.Models.Transactions
{
    public class TransactionTypeModel: BaseEntity
    {
        public string Name { get; set; }

        public bool Active { get; set; }
    }
}
