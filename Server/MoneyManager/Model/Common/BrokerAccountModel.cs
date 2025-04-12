using System;
using MoneyManager.Common;

namespace MoneyManager.WEB.Model
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
    }
}
