namespace MoneyManager.WebApi.Models.Brokers
{
    public class BrokerAccountMonthTransferModel
    {
        public int MonthIndex { get; set; }

        public decimal TotalDeposited { get; set; }

        public decimal TotalWithdrawn { get; set; }
    }
}
