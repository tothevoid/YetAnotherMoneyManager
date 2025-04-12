using System;
using MoneyManager.Common;

namespace MoneyManager.DAL.Entities
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
    }
}
