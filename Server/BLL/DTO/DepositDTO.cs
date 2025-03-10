﻿using MoneyManager.Common;
using System;

namespace MoneyManager.BLL.DTO
{
    public class DepositDTO : BaseEntity
    {
        public string Name { get; set; }

        public DateOnly From { get; set; }

        public DateOnly To { get; set; }

        public float Percentage { get; set; }

        public decimal InitialAmount { get; set; }

        public decimal EstimatedEarn { get; set; }
    }
}