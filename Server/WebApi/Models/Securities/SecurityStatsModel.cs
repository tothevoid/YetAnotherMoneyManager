namespace MoneyManager.WebApi.Models.Securities
{
    public class SecurityStatsModel
    {
        public decimal TransactionsMin { get; set; }

        public decimal TransactionsMax { get; set; }

        public decimal TransactionsAvg { get; set; }

        public int HasOnBrokerAccounts { get; set; }
        
        public decimal DividendsIncome { get; set; }
    }
}
