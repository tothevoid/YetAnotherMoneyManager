using System;
using MoneyManager.Common;

namespace MoneyManager.Model.Deposits
{
    public class ClientDepositModel: CommonDeposit
    {
        public Guid CurrencyId { get; set; }
    }
}
