using System.Collections.Generic;

namespace MoneyManager.Application.DTO.Dashboard
{
    public class GlobalDashboardDto
    {
        public decimal Total { get; set; }
        public TransactionsGlobalDashboardDto TransactionStats { get; set; } = new TransactionsGlobalDashboardDto();
        public BrokerAccountsGlobalDashboardDto BrokerAccountStats { get; set; } = new BrokerAccountsGlobalDashboardDto();
        public AccountsGlobalDashboardDto AccountsGlobalDashboard { get; set; } = new AccountsGlobalDashboardDto();
        public DebtsGlobalDashboardDto DebtsGlobalDashboard { get; set; } = new DebtsGlobalDashboardDto();
        public DepositsGlobalDashboardDto DepositsGlobalDashboardDto { get; set; } = new DepositsGlobalDashboardDto();
        public CryptoAccountsGlobalDashboardDto CryptoAccountsGlobalDashboard { get; set; } = new CryptoAccountsGlobalDashboardDto();
    }

    public class TransactionsGlobalDashboardDto
    {
        public decimal SpentsTotal { get; set; }

        public decimal IncomesTotal { get; set; }

        public IEnumerable<DistributionDto> SpentsDistribution { get; set; }

        public IEnumerable<DistributionDto> IncomesDistribution { get; set; }
    }

    public class BrokerAccountsGlobalDashboardDto
    {
        public decimal Total { get; set; }

        public IEnumerable<DistributionDto> Distribution { get; set; }
    }

    public class DebtsGlobalDashboardDto
    {
        public decimal Total { get; set; }

        public IEnumerable<DistributionDto> Distribution { get; set; }
    }

    public class DepositsGlobalDashboardDto
    {
        public decimal Total { get; set; }

        public decimal TotalStartedAmount { get; set; }

        public decimal TotalEarned { get; set; }

        public IEnumerable<DistributionDto> StartedAmountDistribution { get; set; }

        public IEnumerable<DistributionDto> EarningsDistribution { get; set; }
    }

    public class AccountsGlobalDashboardDto 
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

    public class CryptoAccountsGlobalDashboardDto
    {
        public decimal Total { get; set; }

        public IEnumerable<DistributionDto> Distribution { get; set; }
    }
}
