using System;
using MoneyManager.Application.DTO.Currencies;
using MoneyManager.Shared.Entities;

namespace MoneyManager.Application.DTO.Brokers
{
    public class BrokerAccountDto: BaseEntity
    {
        public string Name { get; set; }

        public BrokerAccountTypeDto Type { get; set; }

        public Guid TypeId { get; set; }

        public CurrencyDTO Currency { get; set; }

        public Guid CurrencyId { get; set; }

        public BrokerDto Broker { get; set; }

        public Guid BrokerId { get; set; }
    }
}
