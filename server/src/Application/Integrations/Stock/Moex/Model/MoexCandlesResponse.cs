using System.Text.Json.Serialization;

namespace MoneyManager.Application.Integrations.Stock.Moex.Model
{
    public class MoexCandlesResponse
    {
        [JsonPropertyName("candles")]
        public DynamicMoexResponseObject Candles { get; set; }
    }
}
