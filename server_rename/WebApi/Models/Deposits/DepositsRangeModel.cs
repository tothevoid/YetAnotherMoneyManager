using System;

namespace MoneyManager.WebApi.Models.Deposits
{
    public class DepositsRangeModel
    {
        public DateOnly From { get; set; }

        public DateOnly To { get; set; }
    }
}