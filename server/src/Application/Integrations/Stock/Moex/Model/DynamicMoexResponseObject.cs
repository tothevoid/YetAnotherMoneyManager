using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Audex.Application.Integrations.Stock.Moex.Model
{
    public class DynamicMoexResponseObject
    {
        [JsonPropertyName("columns")]
        public string[] Columns { get; set; }

        [JsonPropertyName("data")]
        public IEnumerable<object[]> Data { get; set; }
    }
}