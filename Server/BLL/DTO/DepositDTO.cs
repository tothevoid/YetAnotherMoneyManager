using MoneyManager.Common;
using System;

namespace MoneyManager.BLL.DTO
{
    public class DepositDTO : BaseEntity
    {
        public string Name { get; set; }

        public DateTime From { get; set; }

        public DateTime To { get; set; }
    }
}