using MoneyManager.Common;
using System;

namespace MoneyManager.BLL.DTO
{
    public class UpdateAccountDTO
    {
        public Guid AccountId { get; set; }

        public decimal Delta { get; set; }
    }
}
