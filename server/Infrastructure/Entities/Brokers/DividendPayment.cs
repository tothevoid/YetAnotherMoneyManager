using System;
using MoneyManager.Infrastructure.Entities.Securities;
using MoneyManager.Shared.Entities;

namespace MoneyManager.Infrastructure.Entities.Brokers
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
