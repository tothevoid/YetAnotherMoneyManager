using System;
using MoneyManager.WebApi.Models.Accounts;

namespace MoneyManager.WebApi.Models.Deposits
{
    public class ServerDepositModel: CommonDeposit
    {
        public Guid AccountId { get; set; }

        public AccountModel Account { get; set; }
    }
}
