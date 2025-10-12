using System;
using MoneyManager.Shared.Entities;
using MoneyManager.WebApi.Models.Accounts;

namespace MoneyManager.WebApi.Models.Brokers
{
    public class BrokerAccountFundsTransferModel: BaseEntity
    {
        public decimal Amount { get; set; }

        public AccountModel Account { get; set; }

        public Guid AccountId { get; set; }

        public BrokerAccountModel BrokerAccount { get; set; }

        public Guid BrokerAccountId { get; set; }

        public bool Income { get; set; }
    }
}
