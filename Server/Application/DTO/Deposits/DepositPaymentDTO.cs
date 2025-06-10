using System;

namespace MoneyManager.Application.DTO.Deposits
{
    public class DepositPaymentDTO
    {
        public Guid DepositId { get; set; }

        public string Name { get; set; }

        public decimal Value { get; set; }
    }
}
