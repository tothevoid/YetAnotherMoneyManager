﻿using System;
using MoneyManager.Infrastructure.Entities.Currencies;
using MoneyManager.Shared.Entities;

namespace MoneyManager.Infrastructure.Entities.Securities
{
    public class Security: BaseEntity
    {
        public string Name { get; set; }

        public string Ticker { get; set; }

        public SecurityType Type { get; set; }

        public Guid TypeId { get; set; }

        public decimal ActualPrice { get; set; }

        public DateTime PriceFetchedAt { get; set; }

        public string IconKey { get; set; }

        public Currency Currency { get; set; }

        public Guid CurrencyId { get; set; }
    }
}
