using System;

namespace MoneyManager.Application.Integrations.Stock.Moex.Model
{
    public class SecurityRow
    {
        public string Ticker { get; set; }

        public string BoardId { get; set; }

        public decimal? PrevPrice { get; set; }

        public string GetUniqueKey()
        {
            return $"{Ticker}_{BoardId}";
        }
    }
}
