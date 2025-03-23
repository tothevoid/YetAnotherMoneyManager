using MoneyManager.Common;
using System;

namespace MoneyManager.WEB.Model
{
    public class AccountTypeModel : BaseEntity
    {
        public string Name { get; set; }

        public bool Active { get; set; }
    }
}