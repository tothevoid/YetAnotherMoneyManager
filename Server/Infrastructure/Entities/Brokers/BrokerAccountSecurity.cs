using System;
using MoneyManager.Infrastructure.Entities.Securities;
using MoneyManager.Shared.Entities;

namespace MoneyManager.Infrastructure.Entities.Brokers
{
    public class BrokerAccountSecurity: BaseEntity
    {
        public BrokerAccount BrokerAccount { get; set; }

        public Guid BrokerAccountId { get; set; }

        public Security Security { get; set; }

        public Guid SecurityId { get; set; }

        public decimal Price { get; set; }

        public int Quantity { get; set; }

    }
}
