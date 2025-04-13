using System;
using MoneyManager.Application.DTO.Currencies;
using MoneyManager.Shared.Entities;

namespace MoneyManager.Application.DTO.Brokers
{
    public class BrokerAccountDTO: BaseEntity
    {
        public string Name { get; set; }

        public BrokerAccountTypeDTO Type { get; set; }

        public Guid TypeId { get; set; }

        public CurrencyDTO Currency { get; set; }

        public Guid CurrencyId { get; set; }

        public BrokerDTO Broker { get; set; }

        public Guid BrokerId { get; set; }

        public DateOnly LastUpdateAt { get; set; }

        public decimal AssetsValue { get; set; }
    }
}
