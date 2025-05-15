using System;
using MoneyManager.Shared.Entities;

namespace MoneyManager.Application.DTO.Transactions
{
    public class TransactionTypeDTO: BaseEntity
    {
        public string Name { get; set; }
    }
}
