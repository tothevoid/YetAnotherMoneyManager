using System;
using MoneyManager.Application.DTO.Accounts;
using MoneyManager.BLL.DTO;

namespace MoneyManager.Model.Deposits
{
    public class ServerDepositDTO : CommonDepositDTO
    {
        public Guid AccountId { get; set; }

        public AccountDTO Account { get; set; }
    }
}
