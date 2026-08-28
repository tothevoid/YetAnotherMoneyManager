using System;
using Audex.Shared.Entities;
using Audex.WebApi.Models.Accounts;

namespace Audex.WebApi.Models.Brokers
{
    public class BrokerAccountFundsTransferModel: BaseEntity
    {
        public DateTime Date { get; set; }

        public decimal Amount { get; set; }

        public AccountModel Account { get; set; }

        public Guid AccountId { get; set; }

        public BrokerAccountModel BrokerAccount { get; set; }

        public Guid BrokerAccountId { get; set; }

        public bool Income { get; set; }
    }
}
