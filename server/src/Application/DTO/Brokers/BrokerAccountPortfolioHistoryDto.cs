using System;

namespace MoneyManager.Application.DTO.Brokers
{
    public class BrokerAccountPortfolioHistoryDto
    {
        public DateOnly Date { get; set; }

        public decimal MainCurrencyAmount { get; set; }

        public decimal PortfolioValue { get; set; }

        public decimal TotalDividends { get; set; }

        public decimal TotalTaxDeduction { get; set; }

        public decimal TotalDeposited { get; set; }

        public decimal TotalWithdraw { get; set; }

        public decimal ProfitAndLoss { get; set; }
    }
}
