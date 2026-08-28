using Audex.Shared.Entities;
using Audex.WebApi.Models.Currencies;
using System;
using Audex.WebApi.Models.Banks;

namespace Audex.WebApi.Models.Deposits
{
    public class DepositModel: BaseEntity
    {
        public string Name { get; set; }

        public DateOnly From { get; set; }

        public DateOnly To { get; set; }

        public decimal Percentage { get; set; }

        public decimal InitialAmount { get; set; }

        public decimal EstimatedEarn { get; set; }

        public CurrencyModel Currency { get; set; }

        public Guid CurrencyId { get; set; }

        public Guid? BankId { get; set; }

        public BankModel Bank { get; set; }
    }
}
