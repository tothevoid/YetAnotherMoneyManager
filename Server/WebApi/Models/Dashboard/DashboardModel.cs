using MoneyManager.Application.DTO.Dashboard;
using System.Collections.Generic;

namespace MoneyManager.WebApi.Models.Dashboard
{
    public class DashboardModel
    {
        public decimal Total { get; set; }

        public TransactionStatsModel TransactionStats { get; set; }

        public BrokerAccountStatsModel BrokerAccountStats { get; set; }

        public AccountStatsModel AccountStats { get; set; }

        public DebtStatsModel DebtStats { get; set; }

        public DepositStats DepositStats { get; set; }

        public CryptoAccountStatsModel CryptoAccountStats { get; set; }
    }

    public class TransactionStatsModel
    {
        public decimal SpentsTotal { get; set; }

        public decimal IncomesTotal { get; set; }

        public IEnumerable<DistributionModel> SpentsDistribution { get; set; }

        public IEnumerable<DistributionModel> IncomesDistribution { get; set; }
    }

    public class BrokerAccountStatsModel
    {
        public decimal Total { get; set; }

        public IEnumerable<DistributionModel> Distribution { get; set; }
    }

    public class DebtStatsModel
    {
        public decimal Total { get; set; }

        public IEnumerable<DistributionModel> Distribution { get; set; }
    }

    public class DepositStatsModel
    {
        public decimal Total { get; set; }

        public decimal TotalStartedAmount { get; set; }

        public decimal TotalEarned { get; set; }

        public IEnumerable<DistributionModel> StartedAmountDistribution { get; set; }

        public IEnumerable<DistributionModel> EarningsDistribution { get; set; }
    }

    public class AccountStatsModel
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

    public class CryptoAccountStatsModel
    {
        public decimal Total { get; set; }

        public IEnumerable<DistributionModel> Distribution { get; set; }
    }
}
