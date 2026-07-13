namespace MoneyManager.WebApi.Models.Brokers
{
    public class BrokerAccountPortfolioModel
    {
        public decimal CurrentAmount { get; set; }

        public decimal DividendsIncome { get; set; }

        public decimal TaxDeductions { get; set; }

        public decimal ProfitAndLoss { get; set; }
    }
}
