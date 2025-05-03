using System;
using MoneyManager.Shared.Entities;
using MoneyManager.WebApi.Models.Currencies;

namespace MoneyManager.WebApi.Models.Securities
{
    public class SecurityModel: BaseEntity
    {
        public string Name { get; set; }

        public string Ticker { get; set; }

        public SecurityTypeModel Type { get; set; }

        public Guid TypeId { get; set; }

        public decimal ActualPrice { get; set; }

        public DateTime PriceFetchedAt { get; set; }
    }
}
