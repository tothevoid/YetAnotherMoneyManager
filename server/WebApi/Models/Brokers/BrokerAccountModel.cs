using MoneyManager.Shared.Entities;
using MoneyManager.WebApi.Models.Currencies;
using System;
using MoneyManager.WebApi.Models.Banks;

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

        public BankModel Bank { get; set; }

        public Guid? BankId { get; set; }

        public decimal InitialValue { get; set; }

         public decimal CurrentValue { get; set; }

         public decimal MainCurrencyAmount { get; set; }
    }
}
