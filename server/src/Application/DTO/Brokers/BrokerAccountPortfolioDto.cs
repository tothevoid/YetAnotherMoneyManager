namespace MoneyManager.Application.DTO.Brokers
{
    public class BrokerAccountPortfolioDto
    {
        public decimal CurrentAmount { get; set; }

        public decimal DividendsIncome { get;set; }

        public decimal TaxDeductions { get;set; }

        public decimal ProfitAndLoss { get;set; }

        public decimal MainCurrencyAmount { get; set;}
    }
}
