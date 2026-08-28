using System;
using Audex.Infrastructure.Entities.Securities;
using Audex.Shared.Entities;

namespace Audex.Infrastructure.Entities.Brokers
{
    public class DividendPayment: BaseEntity
    {
        public BrokerAccount BrokerAccount { get; set; }

        public Guid BrokerAccountId { get; set; }

        public Dividend Dividend { get; set; }

        public Guid DividendId { get; set; }

        public int SecuritiesQuantity { get; set; }

        public decimal Tax { get; set; }

        public DateOnly ReceivedAt { get; set; }
    }
}
