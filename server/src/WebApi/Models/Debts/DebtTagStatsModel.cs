using System;

namespace Audex.WebApi.Models.Debts
{
    public class DebtTagStatsModel
    {
        public Guid TagId { get; set; }

        public string TagName { get; set; } = string.Empty;

        public string ColorHex { get; set; } = "#3182CE";

        public decimal TotalAmount { get; set; }

        public decimal TotalPaid { get; set; }

        public decimal RemainingAmount { get; set; }

        public string CurrencyName { get; set; } = string.Empty;

        public double RepaymentPercentage { get; set; }
    }
}
