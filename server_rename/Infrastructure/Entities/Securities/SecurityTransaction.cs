using System;
using MoneyManager.Infrastructure.Entities.Brokers;
using MoneyManager.Shared.Entities;

namespace MoneyManager.Infrastructure.Entities.Securities
{
    public class SecurityTransaction: BaseEntity
    {
        public Security Security { get; set; }

        public Guid SecurityId { get; set; }

        public BrokerAccount BrokerAccount { get; set; }

        public Guid BrokerAccountId { get; set; }

        public int Quantity { get; set; }

        public decimal Price { get; set; }

        public DateTime Date { get; set; }

        public decimal BrokerCommission { get; set; }

        public decimal StockExchangeCommission { get; set; }

        public decimal Tax { get; set; }
    }
}
