using Audex.Application.DTO.Currencies;
using Audex.Infrastructure.Entities.Banks;
using Audex.Shared.Entities;
using System;
using Audex.Application.DTO.Banks;

namespace Audex.Application.DTO.Deposits
{
    public class DepositDto : BaseEntity
    {
        public string Name { get; set; }

        public DateOnly From { get; set; }

        public DateOnly To { get; set; }

        public decimal Percentage { get; set; }

        public decimal InitialAmount { get; set; }

        public decimal EstimatedEarn { get; set; }

        public CurrencyDto Currency { get; set; }

        public Guid CurrencyId { get; set; }

        public Guid? BankId { get; set; }

        public BankDto Bank { get; set; }
    }
}