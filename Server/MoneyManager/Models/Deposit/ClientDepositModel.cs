using System;

namespace MoneyManager.WebApi.Models.Deposit
{
    public class ClientDepositModel: CommonDeposit
    {
        public Guid CurrencyId { get; set; }
    }
}
