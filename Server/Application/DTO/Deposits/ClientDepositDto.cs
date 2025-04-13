using System;

namespace MoneyManager.BLL.DTO
{
    public class ClientDepositDTO: CommonDepositDTO
    {
        public Guid CurrencyId { get; set; }
    }
}
