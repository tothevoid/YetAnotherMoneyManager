namespace MoneyManager.WebApi.Models.Brokers
{
    public class BrokerAccountDayTransferModel
    {
        public int DayIndex { get; set; }

        public decimal TotalDeposited { get; set; }

        public decimal TotalWithdrawn { get; set; }
    }
}
