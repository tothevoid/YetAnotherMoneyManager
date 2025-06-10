﻿using MoneyManager.Shared.Entities;
using System;
using MoneyManager.Application.DTO.Currencies;

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
    }
}