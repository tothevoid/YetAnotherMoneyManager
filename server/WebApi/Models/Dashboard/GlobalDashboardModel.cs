using MoneyManager.Application.DTO.Dashboard;
using System.Collections.Generic;

namespace MoneyManager.WebApi.Models.Dashboard
{
    public class GlobalDashboardModel
    {
        public decimal Total { get; set; }

        public TransactionsGlobalDashboardModel TransactionsGlobalDashboard { get; set; }

        public BrokerAccountsGlobalDashboardModel BrokerAccountsGlobalDashboard { get; set; }

        public AccountsGlobalDashboardModel AccountsGlobalDashboard { get; set; }

        public DebtsGlobalDashboardModel DebtsGlobalDashboard { get; set; }

        public DepositsGlobalDashboardDto DepositsGlobalDashboardDto { get; set; }

        public CryptoAccountsGlobalDashboardModel CryptoAccountsGlobalDashboard { get; set; }
    }

    public class TransactionsGlobalDashboardModel
    {
        public decimal SpentsTotal { get; set; }

        public decimal IncomesTotal { get; set; }

        public IEnumerable<DistributionModel> SpentsDistribution { get; set; }

        public IEnumerable<DistributionModel> IncomesDistribution { get; set; }
    }

    public class BrokerAccountsGlobalDashboardModel
    {
        public decimal Total { get; set; }

        public IEnumerable<DistributionModel> Distribution { get; set; }
    }

    public class DebtsGlobalDashboardModel
    {
        public decimal Total { get; set; }

        public IEnumerable<DistributionModel> Distribution { get; set; }
    }

    public class DepositsGlobalDashboardModel
    {
        public decimal Total { get; set; }

        public decimal TotalStartedAmount { get; set; }

        public decimal TotalEarned { get; set; }

        public IEnumerable<DistributionModel> StartedAmountDistribution { get; set; }

        public IEnumerable<DistributionModel> EarningsDistribution { get; set; }
    }

    public class AccountsGlobalDashboardModel
    {
        public decimal Total { get; set; }

        public decimal TotalCash { get; set; }

        public decimal TotalDeposit { get; set; }

        public decimal TotalBankAccount { get; set; }

        public IEnumerable<DistributionModel> CashDistribution { get; set; }

        public IEnumerable<DistributionModel> BankAccountsDistribution { get; set; }
    }

    public class DistributionModel
    {
        public string Name { get; set; }

        public string Currency { get; set; }

        public decimal Amount { get; set; }

        public decimal ConvertedAmount { get; set; }
    }

    public class CryptoAccountsGlobalDashboardModel
    {
        public decimal Total { get; set; }

        public IEnumerable<DistributionModel> Distribution { get; set; }
    }
}
