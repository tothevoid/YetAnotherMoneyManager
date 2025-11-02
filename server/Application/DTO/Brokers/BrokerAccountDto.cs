using MoneyManager.Application.DTO.Currencies;
using MoneyManager.Shared.Entities;
using System;
using MoneyManager.Application.DTO.Banks;

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

        public BankDto Bank { get; set; }

        public Guid? BankId { get; set; }

        public decimal InitialValue { get; set; }

        public decimal CurrentValue { get; set; }

        public decimal MainCurrencyAmount { get; set; }

        public void ApplyPortfolioValues(BrokerAccountPortfolioDto portfolioDto)
        {
            InitialValue = portfolioDto.InitialValue;
            CurrentValue = portfolioDto.CurrentValue;
        }
    }
}
