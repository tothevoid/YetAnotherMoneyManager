using MoneyManager.Application.DTO.Currencies;
using MoneyManager.Shared.Entities;
using System;
using MoneyManager.Application.DTO.Banks;

namespace MoneyManager.Application.DTO.Brokers
{
    public class BrokerAccountDto: BaseEntity
    {
        public string Name { get; set; }

        public BrokerAccountTypeDto Type { get; set; }

        public Guid TypeId { get; set; }

        public CurrencyDto Currency { get; set; }

        public Guid CurrencyId { get; set; }

        public BrokerDto Broker { get; set; }

        public Guid BrokerId { get; set; }

        public BankDto Bank { get; set; }

        public Guid? BankId { get; set; }

        public decimal MainCurrencyAmount { get; set; }
    }
}
