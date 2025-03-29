using System;
using MoneyManager.Common;

namespace MoneyManager.Model.Common
{
    public class DepositModel: BaseEntity
    {
        public string Name { get; set; }

        public DateOnly From { get; set; }

        public DateOnly To { get; set; }

        public decimal Percentage { get; set; }

        public decimal InitialAmount { get; set; }

        public decimal EstimatedEarn { get; set; }
    }
}
