using System.Collections.Generic;

namespace MoneyManager.Application.DTO.Dashboard
{
    public class DashboardDto
    {
        public decimal Total { get; set; }
        public TransactionStatsDto TransactionStats { get; set; } = new TransactionStatsDto();
        public BrokerAccountStatsDto BrokerAccountStats { get; set; } = new BrokerAccountStatsDto();
        public AccountStatsDto AccountStats { get; set; } = new AccountStatsDto();
        public DebtStatsDto DebtStats { get; set; } = new DebtStatsDto();
        public DepositStats DepositStats { get; set; } = new DepositStats();
    }

    public class TransactionStatsDto
    {
        public decimal SpentsTotal { get; set; }

        public decimal IncomesTotal { get; set; }

        public IEnumerable<DistributionDto> SpentsDistribution { get; set; }

        public IEnumerable<DistributionDto> IncomesDistribution { get; set; }
    }

    public class BrokerAccountStatsDto
    {
        public decimal Total { get; set; }

        public IEnumerable<DistributionDto> Distribution { get; set; }
    }

    public class DebtStatsDto
    {
        public decimal Total { get; set; }

        public IEnumerable<DistributionDto> Distribution { get; set; }
    }

    public class DepositStats
    {
        public decimal Total { get; set; }

        public decimal TotalStartedAmount { get; set; }

        public decimal TotalEarned { get; set; }

        public IEnumerable<DistributionDto> StartedAmountDistribution { get; set; }

        public IEnumerable<DistributionDto> EarningsDistribution { get; set; }
    }

    public class AccountStatsDto 
    {
        public decimal Total { get; set; }

        public decimal TotalCash { get; set; }

        public decimal TotalDeposit { get; set; }

        public decimal TotalBankAccount { get; set; }

        public IEnumerable<DistributionDto> CashDistribution { get; set; }

        public IEnumerable<DistributionDto> BankAccountsDistribution { get; set; }

    }

    public class DistributionDto
    {
        public string Name { get; set; }

        public string Currency { get; set; }

        public decimal Amount { get; set; }
        
        public decimal ConvertedAmount { get; set; }
    }
}
