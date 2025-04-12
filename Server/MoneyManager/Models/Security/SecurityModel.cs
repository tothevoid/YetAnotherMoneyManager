using System;
using MoneyManager.Shared.Entities;
using MoneyManager.WebApi.Models.Currency;

namespace MoneyManager.WebApi.Models.Security
{
    public class SecurityModel: BaseEntity
    {
        public string Name { get; set; }

        public string Ticker { get; set; }

        public SecurityTypeModel Type { get; set; }

        public Guid TypeId { get; set; }

        public CurrencyModel Currency { get; set; }

        public Guid CurrencyId { get; set; }
    }
}
