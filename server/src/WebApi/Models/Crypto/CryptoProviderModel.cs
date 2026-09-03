using System;
using System.Text.Json.Serialization;

namespace Audex.WebApi.Models.Crypto
{
    public class CryptoProviderModel
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("iconKey")]
        public string IconKey { get; set; }
    }
}
