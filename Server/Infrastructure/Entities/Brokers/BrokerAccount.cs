using System;
using MoneyManager.Infrastructure.Entities.Currencies;
using MoneyManager.Shared.Entities;

namespace MoneyManager.Infrastructure.Entities.Brokers
{
    public class BrokerAccount: BaseEntity
    {
        public string Name { get; set; }

        public BrokerAccountType Type { get; set; }

        public Guid TypeId { get; set; }

        public Currency Currency { get; set; }

        public Guid CurrencyId { get; set; }

        public Broker Broker { get; set; }

        public Guid BrokerId { get; set; }

        public DateTime LastUpdateAt { get; set; }

        public decimal AssetsValue { get; set; }
    }
}
