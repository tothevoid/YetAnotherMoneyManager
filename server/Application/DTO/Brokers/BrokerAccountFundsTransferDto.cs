using MoneyManager.Infrastructure.Entities.Accounts;
using MoneyManager.Infrastructure.Entities.Brokers;
using MoneyManager.Shared.Entities;
using System;
namespace MoneyManager.Application.DTO.Brokers
{
    public class BrokerAccountFundsTransferDto : BaseEntity
    {
        public decimal Amount { get; set; }

        public Account Account { get; set; }

        public Guid AccountId { get; set; }

        public BrokerAccount BrokerAccount { get; set; }

        public Guid BrokerAccountId { get; set; }

        public bool Income { get; set; }
    }
}
