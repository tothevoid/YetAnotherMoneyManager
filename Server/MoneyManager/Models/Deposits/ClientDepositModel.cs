using System;

namespace MoneyManager.WebApi.Models.Deposits
{
    public class ClientDepositModel: CommonDeposit
    {
        public Guid CurrencyId { get; set; }
    }
}
