using System.Text.Json.Serialization;

namespace Audex.Application.Integrations.Stock.Moex.Model
{
    public class MoexCandlesResponse
    {
        [JsonPropertyName("candles")]
        public DynamicMoexResponseObject Candles { get; set; }
    }
}
