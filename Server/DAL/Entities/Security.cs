using System;
using MoneyManager.Common;

namespace MoneyManager.DAL.Entities
{
    public class Security: BaseEntity
    {
        public string Name { get; set; }

        public string Ticker { get; set; }

        public SecurityType Type { get; set; }

        public Guid TypeId { get; set; }

        public Currency Currency { get; set; }

        public Guid CurrencyId { get; set; }
    }
}
