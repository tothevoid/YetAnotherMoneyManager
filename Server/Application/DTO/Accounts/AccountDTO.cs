﻿using MoneyManager.Application.DTO.Currencies;
using MoneyManager.Shared.Entities;
using System;

namespace MoneyManager.Application.DTO.Accounts
{
    public class AccountDTO : BaseEntity
    {
        public string Name { get; set; }

        public decimal Balance { get; set; }

        public CurrencyDTO Currency { get; set; }

        public AccountTypeDTO AccountType { get; set; }

        public Guid CurrencyId { get; set; }

        public Guid AccountTypeId { get; set; }

        public DateOnly CreatedOn { get; set; }

        public bool Active { get; set; }
    }
}
