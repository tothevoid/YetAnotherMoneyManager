using System;

namespace MoneyManager.Application.Integrations.Stock.Moex.Model
{
    public class MarketDataRow
    {
        public string Ticker { get; set; }

        public string BoardId { get; set; }

        public decimal? LastValue { get; set; }

        public DateTime Date { get; set; }

        public decimal? MarketPrice { get; set; }

        public decimal? Open { get; set; }

        public decimal Low { get; set; }

        public decimal High { get; set; }
    }
}
