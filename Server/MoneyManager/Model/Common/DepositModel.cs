using System;
using MoneyManager.Common;

namespace MoneyManager.Model.Common
{
    public class DepositModel: BaseEntity
    {
        public string Name { get; set; }

        public DateTime From { get; set; }

        public DateTime To { get; set; }

        public float Percentage { get; set; }

        public decimal InitialAmount { get; set; }
    }
}
