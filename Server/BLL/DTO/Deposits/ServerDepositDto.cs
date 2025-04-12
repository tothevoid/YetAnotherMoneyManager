using System;
using MoneyManager.BLL.DTO;
using MoneyManager.DAL.Entities;

namespace MoneyManager.Model.Deposits
{
    public class ServerDepositDto : CommonDepositDto
    {
        public Guid AccountId { get; set; }

        public AccountDTO Account { get; set; }
    }
}
