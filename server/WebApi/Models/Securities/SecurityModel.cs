using System;
using System.Text.Json.Serialization;
using MoneyManager.Infrastructure.Entities.Currencies;
using MoneyManager.Shared.Entities;
using MoneyManager.WebApi.Models.Currencies;

namespace MoneyManager.WebApi.Models.Securities
{
    public class SecurityModel
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("ticker")]
        public string Ticker { get; set; }

        [JsonPropertyName("type")]
        public SecurityTypeModel Type { get; set; }

        [JsonPropertyName("typeId")]
        public Guid TypeId { get; set; }

        [JsonPropertyName("actualPrice")]
        public decimal ActualPrice { get; set; }

        [JsonPropertyName("priceFetchedAt")]
        public DateTime PriceFetchedAt { get; set; }

        [JsonPropertyName("iconKey")]
        public string IconKey { get; set; }

        [JsonPropertyName("currency")]
        public Currency Currency { get; set; }

        [JsonPropertyName("currencyId")]
        public Guid CurrencyId { get; set; }
    }
}
