using System;

namespace MoneyManager.Application.DTO
{
    public class ClientDepositDTO: CommonDepositDTO
    {
        public Guid CurrencyId { get; set; }
    }
}
