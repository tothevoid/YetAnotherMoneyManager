﻿using MoneyManager.Common;
using System;

namespace MoneyManager.WEB.Model
{
    public class AccountModel : BaseEntity
    {
        public string Name { get; set; }

        public decimal Balance { get; set; }
        
        public CurrencyModel Currency { get; set; }

        public AccountTypeModel AccountType { get; set; }

        public Guid CurrencyId { get; set; }

        public Guid AccountTypeId { get; set; }

        public DateOnly CreatedOn { get; set; }

        public bool Active { get; set; }
    }
}