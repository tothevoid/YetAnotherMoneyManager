using MoneyManager.Shared.Entities;
using System;
using MoneyManager.Application.DTO.Accounts;

namespace MoneyManager.Application.DTO.Brokers
{
    public class BrokerAccountFundsTransferDto : BaseEntity
    {
        public DateTime Date { get; set; }

        public decimal Amount { get; set; }

        public AccountDto Account { get; set; }

        public Guid AccountId { get; set; }

        public BrokerAccountDto BrokerAccount { get; set; }

        public Guid BrokerAccountId { get; set; }

        public bool Income { get; set; }
    }
}
