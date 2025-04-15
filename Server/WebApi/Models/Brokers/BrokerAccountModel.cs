using System;
using MoneyManager.Shared.Entities;
using MoneyManager.WebApi.Models.Currencies;

namespace MoneyManager.WebApi.Models.Brokers
{
    public class BrokerAccountModel: BaseEntity
    {
        public string Name { get; set; }

        public BrokerAccountTypeModel Type { get; set; }

        public Guid TypeId { get; set; }

        public CurrencyModel Currency { get; set; }

        public Guid CurrencyId { get; set; }

        public BrokerModel Broker { get; set; }

        public Guid BrokerId { get; set; }

        public DateTime LastUpdateAt { get; set; }

        public decimal AssetsValue { get; set; }
    }
}
