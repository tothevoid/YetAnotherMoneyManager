using MoneyManager.Application.DTO.Currencies;
using MoneyManager.Infrastructure.Entities.Banks;
using MoneyManager.Shared.Entities;
using System;
using MoneyManager.Application.DTO.Banks;

namespace MoneyManager.Application.DTO.Deposits
{
    public class DepositDTO : BaseEntity
    {
        public string Name { get; set; }

        public DateOnly From { get; set; }

        public DateOnly To { get; set; }

        public decimal Percentage { get; set; }

        public decimal InitialAmount { get; set; }

        public decimal EstimatedEarn { get; set; }

        public CurrencyDTO Currency { get; set; }

        public Guid CurrencyId { get; set; }

        public Guid? BankId { get; set; }

        public BankDto Bank { get; set; }
    }
}