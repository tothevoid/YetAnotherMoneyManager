using System;
using MoneyManager.Shared.Entities;

namespace MoneyManager.WebApi.Models.Deposit
{
    public class DepositModel: BaseEntity
    {
        public string Name { get; set; }

        public DateOnly From { get; set; }

        public DateOnly To { get; set; }

        public decimal Percentage { get; set; }

        public decimal InitialAmount { get; set; }

        public decimal EstimatedEarn { get; set; }

        public Guid CurrencyId { get; set; }

        public Guid? AccountId { get; set; }
    }
}
