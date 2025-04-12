using System;
using MoneyManager.Common;

namespace MoneyManager.BLL.DTO
{
    public class ClientDepositDto: CommonDepositDto
    {
        public Guid CurrencyId { get; set; }
    }
}
