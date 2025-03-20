using MoneyManager.Common;
using MoneyManager.DAL;
using System;
using MoneyManager.DAL.Entities;

namespace MoneyManager.WEB.Model
{
    public class AccountModel : BaseEntity
    {
        public string Name { get; set; }

        public double Balance { get; set; }
        
        public Currency Currency { get; set; }

        public Guid CurrencyId { get; set; }
    }
}