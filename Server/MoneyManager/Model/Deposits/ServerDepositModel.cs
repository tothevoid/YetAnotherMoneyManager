using System;
using MoneyManager.WEB.Model;

namespace MoneyManager.Model.Deposits
{
    public class ServerDepositModel: CommonDeposit
    {
        public Guid AccountId { get; set; }

        public AccountModel Account { get; set; }
    }
}
