using System;

namespace MoneyManager.Application.Integrations.Stock.Moex.Model
{
    public class SecurityRow
    {
        public string Ticker { get; set; }

        public DateTime UpdateTime { get; set; }

        public string BoardId { get; set; }

        public decimal? PrevPrice { get; set; }
    }
}
