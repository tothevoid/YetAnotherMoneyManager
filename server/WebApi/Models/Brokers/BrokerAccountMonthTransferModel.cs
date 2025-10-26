namespace MoneyManager.WebApi.Models.Brokers
{
    public class BrokerAccountMonthTransferModel
    {
        public int MonthIndex { get; set; }

        public bool Income { get; set; }

        public decimal Value { get; set; }
    }
}
