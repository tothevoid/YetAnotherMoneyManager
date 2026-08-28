using System;

namespace MoneyManager.Application.DTO.Deposits
{
    public class DepositPaymentDto
    {
        public Guid DepositId { get; set; }

        public string Name { get; set; }

        public decimal Value { get; set; }
    }
}
