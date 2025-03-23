using MoneyManager.Common;
using System;

namespace MoneyManager.BLL.DTO
{
    public class AccountTypeDTO : BaseEntity
    {
        public string Name { get; set; }

        public bool Active { get; set; }
    }
}