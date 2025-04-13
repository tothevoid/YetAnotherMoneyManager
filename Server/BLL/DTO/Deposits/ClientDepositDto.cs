using System;

namespace MoneyManager.BLL.DTO
{
    public class ClientDepositDto: CommonDepositDto
    {
        public Guid CurrencyId { get; set; }
    }
}
