using MoneyManager.Shared.Entities;
using System;
using System.Text.Json.Serialization;

namespace MoneyManager.WebApi.Models.Banks
{
    public class BankModel
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("iconKey")]
        public string IconKey { get; set; }
    }
}