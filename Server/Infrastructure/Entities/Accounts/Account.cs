﻿using MoneyManager.Infrastructure.Entities.Currencies;
using MoneyManager.Shared.Entities;
using System;

namespace MoneyManager.Infrastructure.Entities.Accounts
{
    public class Account : BaseEntity
    {
        public string Name { get; set; }

        public decimal Balance { get; set; }

        public Currency Currency { get; set; }

        public AccountType AccountType { get; set; }

        public Guid CurrencyId { get; set; }

        public Guid AccountTypeId { get; set; }

        public DateOnly CreatedOn { get; set; }

        public bool Active { get; set; }

        public Account AssignReferences(Currency currency, AccountType accountType)
        {
            Currency = currency;
            AccountType = accountType;
            return this;
        }
    }
}
