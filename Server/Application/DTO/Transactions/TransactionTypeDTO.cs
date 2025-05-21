using System;
using MoneyManager.Shared.Entities;

namespace MoneyManager.Application.DTO.Transactions
{
    public class TransactionTypeDTO: BaseEntity
    {
        public string Name { get; set; }

        public bool Active { get; set; }

        public string IconKey { get; set; }
    }
}
