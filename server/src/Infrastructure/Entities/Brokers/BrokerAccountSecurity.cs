using System;
using Audex.Infrastructure.Entities.Securities;
using Audex.Shared.Entities;

namespace Audex.Infrastructure.Entities.Brokers
{
    public class BrokerAccountSecurity: BaseEntity
    {
        public BrokerAccount BrokerAccount { get; set; }

        public Guid BrokerAccountId { get; set; }

        public Security Security { get; set; }

        public Guid SecurityId { get; set; }

        public decimal Price { get; set; }

        public int Quantity { get; set; }

        public decimal SoldPrice { get; set; }

        public int SoldQuantity { get; set; }
    }
}
