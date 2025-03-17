using MoneyManager.Common;
using MoneyManager.DAL;
using System;

namespace MoneyManager.WEB.Model
{
    public class AccountModel : BaseEntity
    {
        public string Name { get; set; }

        public double Balance { get; set; }
    }
}