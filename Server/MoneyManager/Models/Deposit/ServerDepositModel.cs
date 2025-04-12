using System;
using MoneyManager.WebApi.Models.Account;

namespace MoneyManager.WebApi.Models.Deposit
{
    public class ServerDepositModel: CommonDeposit
    {
        public Guid AccountId { get; set; }

        public AccountModel Account { get; set; }
    }
}
