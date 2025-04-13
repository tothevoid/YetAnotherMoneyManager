using MoneyManager.Shared.Entities;
using System;

namespace MoneyManager.BLL.DTO
{
    public class CommonDepositDTO: BaseEntity
    {
        public string Name { get; set; }

        public DateOnly From { get; set; }

        public DateOnly To { get; set; }

        public decimal Percentage { get; set; }

        public decimal InitialAmount { get; set; }

        public decimal EstimatedEarn { get; set; }
    }
}
