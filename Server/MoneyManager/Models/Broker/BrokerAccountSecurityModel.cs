using System;
using MoneyManager.Shared.Entities;
using MoneyManager.WebApi.Models.Security;

namespace MoneyManager.WebApi.Models.Broker
{
    public class BrokerAccountSecurityModel: BaseEntity
    {
        public BrokerAccountModel BrokerAccount { get; set; }

        public Guid BrokerAccountId { get; set; }

        public SecurityModel Security { get; set; }

        public Guid SecurityId { get; set; }

        public int Quantity { get; set; }

        public decimal InitialPrice { get; set; }

        public decimal CurrentPrice { get; set; }
    }
}
