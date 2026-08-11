using System;

namespace MoneyManager.Application.DTO.Debts
{
    public class DebtTagStatsDto
    {
        public Guid TagId { get; set; }

        public string TagName { get; set; } = string.Empty;

        public string ColorHex { get; set; } = "#3182CE";

        public decimal TotalAmount { get; set; }

        public decimal TotalPaid { get; set; }

        public decimal RemainingAmount { get; set; }

        public double RepaymentPercentage => TotalAmount > 0 ? (double)(TotalPaid / TotalAmount * 100) : 0;
    }
}
