namespace MoneyManager.Application.DTO.Brokers
{
    public class BrokerAccountSummaryDto
    {
        public BrokerAccountTransfersStatsDto TransferStats { get; set; }

        public BrokerAccountStatsDto BrokerAccountStats { get; set; }
    }

    public class BrokerAccountStatsDto
    {
        public decimal InvestedValue { get; set; }

        public decimal CurrentValue { get; set; }

        public decimal TotalDividendsValue { get; set; }
    }

    public class BrokerAccountTransfersStatsDto
    {
        public decimal TotalDeposited { get; set; }

        public decimal TotalWithdrawn { get; set; }
    }
}
