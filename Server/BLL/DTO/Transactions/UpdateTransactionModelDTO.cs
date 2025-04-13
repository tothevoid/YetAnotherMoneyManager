using System;

namespace MoneyManager.Application.DTO.Transactions
{
    public class UpdateAccountDTO
    {
        public Guid AccountId { get; set; }

        public decimal Delta { get; set; }
    }
}
