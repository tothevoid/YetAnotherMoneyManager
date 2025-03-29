using MoneyManager.Common;
using System;

namespace MoneyManager.WEB.Model
{
    public class DepositsRangeModel
    {
        public DateOnly From { get; set; }

        public DateOnly To { get; set; }
    }
}