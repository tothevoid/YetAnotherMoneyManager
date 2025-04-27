using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MoneyManager.Application.Integrations.Stock.Moex.Model
{
    public class MarketData
    {
        [JsonPropertyName("columns")]
        public string[] Columns { get; set; }

        [JsonPropertyName("data")]
        public IEnumerable<string[]> Data { get; set; }
    }
}