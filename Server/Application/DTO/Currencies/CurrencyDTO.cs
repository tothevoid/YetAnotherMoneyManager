﻿using System;
using MoneyManager.Shared.Entities;

namespace MoneyManager.Application.DTO.Currencies
{
    public class CurrencyDTO : BaseEntity
    {
        public string Name { get; set; }

        public bool Active { get; set; }

        public decimal Rate { get; set; }
    }
}
