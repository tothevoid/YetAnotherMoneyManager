using System;
using MoneyManager.Common;

namespace MoneyManager.DAL.Entities
{
    public class BrokerAccountSecurity: BaseEntity
    {
        public BrokerAccount BrokerAccount { get; set; }

        public Guid BrokerAccountId { get; set; }

        public Security Security { get; set; }

        public Guid SecurityId { get; set; }

        public int Quantity { get; set; }

        public decimal InitialPrice { get; set; }

        public decimal CurrentPrice { get; set; }
    }
}
