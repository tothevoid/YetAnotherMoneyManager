using System.Text.Json.Serialization;

namespace Audex.Application.Integrations.Stock.Moex.Model
{
    public class MoexResponse
    {
        [JsonPropertyName("marketdata")]
        public DynamicMoexResponseObject MarketData { get; set; }

        [JsonPropertyName("securities")]
        public DynamicMoexResponseObject Securities { get; set; }
    }
}
