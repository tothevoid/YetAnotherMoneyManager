using Audex.Shared.Entities;
using Audex.WebApi.Models.Currencies;
using System;
using Audex.WebApi.Models.Banks;

namespace Audex.WebApi.Models.Accounts
{
    public class AccountModel : BaseEntity
    {
        public string Name { get; set; }

        public decimal Balance { get; set; }
        
        public CurrencyModel Currency { get; set; }

        public AccountTypeModel AccountType { get; set; }

        public Guid CurrencyId { get; set; }

        public Guid AccountTypeId { get; set; }

        public BankModel Bank { get; set; }

        public Guid? BankId { get; set; }

        public DateOnly CreatedOn { get; set; }

        public bool Active { get; set; }
    }
}