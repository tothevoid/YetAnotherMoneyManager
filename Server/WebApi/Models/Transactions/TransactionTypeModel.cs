using System;
using System.Text.Json.Serialization;
using MoneyManager.Shared.Entities;

namespace MoneyManager.WebApi.Models.Transactions
{
    public class TransactionTypeModel
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("active")]
        public bool Active { get; set; }

        [JsonPropertyName("iconKey")]
        public string IconKey { get; set; }
    }
}
