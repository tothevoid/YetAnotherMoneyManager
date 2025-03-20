using MoneyManager.Common;
using System;

namespace MoneyManager.WEB.Model
{
    public class AccountModel : BaseEntity
    {
        public string Name { get; set; }

        public double Balance { get; set; }
        
        public CurrencyModel Currency { get; set; }

        public Guid CurrencyId { get; set; }
    }
}