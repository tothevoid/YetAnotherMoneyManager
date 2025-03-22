using MoneyManager.Common;
using System;

namespace MoneyManager.BLL.DTO
{
    public class AccountDTO : BaseEntity
    {
        public string Name { get; set; }

        public double Balance { get; set; }

        public CurrencyDTO Currency { get; set; }

        public Guid CurrencyId { get; set; }

        public DateOnly CreatedOn { get; set; }

        public bool Active { get; set; }
    }
}
