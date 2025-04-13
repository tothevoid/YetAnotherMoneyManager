using System;

namespace MoneyManager.Application.DTO.Transactions
{
    public class TransactionTypeDTO
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Extension { get; set; }
    }
}
