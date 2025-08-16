using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MoneyManager.Application.Integrations.Stock.Moex.Model
{
    public class TickerHistory
    {
        [JsonPropertyName("columns")]
        public string[] Columns { get; set; }

        [JsonPropertyName("data")]
        public IEnumerable<object[]> Data { get; set; }
    }
}
