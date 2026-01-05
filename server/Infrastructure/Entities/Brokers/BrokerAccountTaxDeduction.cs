using System;
using MoneyManager.Shared.Entities;

namespace MoneyManager.Infrastructure.Entities.Brokers
{
    public class BrokerAccountTaxDeduction : BaseEntity
    {
        public string Name { get; set; }

        public decimal Amount { get; set; }

        public DateTime DateApplied { get; set; }

        public BrokerAccount BrokerAccount { get; set; }

        public Guid BrokerAccountId { get; set; }
    }
}