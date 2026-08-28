using Audex.Shared.Entities;
using Audex.WebApi.Models.Currencies;
using System;
using Audex.WebApi.Models.Banks;

namespace Audex.WebApi.Models.Brokers
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

        public BankModel Bank { get; set; }

        public Guid? BankId { get; set; }

        public decimal MainCurrencyAmount { get; set; }
    }
}
