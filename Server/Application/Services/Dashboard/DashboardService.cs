using AutoMapper;
using System.Collections.Generic;
using System.Threading.Tasks;
using MoneyManager.Infrastructure.Interfaces.Database;
using MoneyManager.Application.DTO.Dashboard;
using MoneyManager.Application.Interfaces.Currencies;
using MoneyManager.Infrastructure.Constants;
using MoneyManager.Application.Interfaces.Accounts;
using MoneyManager.Application.DTO.Accounts;
using MoneyManager.Application.Interfaces.Deposits;
using MoneyManager.Application.Interfaces.Transactions;
using System;
using System.Linq;
using MoneyManager.Application.Interfaces.Brokers;
using MoneyManager.Application.DTO.Transactions;
using MoneyManager.Application.Interfaces.Debts;
using MoneyManager.Application.Interfaces.User;

namespace MoneyManager.Application.Services.Dashboard
{
    public class DashboardService : IDashboardService
    {
        private readonly IUnitOfWork _db;
        private readonly IMapper _mapper;
        private readonly IAccountService _accountService;
        private readonly IDepositService _depositService;
        private readonly ITransactionsService _transactionsService;
        private readonly IBrokerAccountService _brokerAccountService;
        private readonly IDebtService _debtService;
        private readonly IUserProfileService _userProfile;

        public DashboardService(IUnitOfWork uow, IMapper mapper, IAccountService accountService, 
            IDepositService depositService, ITransactionsService transactionsService,
            IBrokerAccountService brokerAccountService, IDebtService debtService, 
            IUserProfileService userProfile)
        {
            _db = uow;
            _mapper = mapper;
            _accountService = accountService;
            _depositService = depositService;
            _transactionsService = transactionsService;
            _brokerAccountService = brokerAccountService;
            _debtService = debtService;
            _userProfile = userProfile;
        }

        public async Task<DashboardDto> GetDashboard()
        {
            var accountStats = await GetAccountData();
            var brokerAccountStats = await GetBrokerAccountData();
            var debtsStats = await GetDebtsData();
            var depositsIncomes = await GetDepositsIncomesData();

            return new DashboardDto()
            {
                //TODO: make it via setters
                Total = accountStats.Total + brokerAccountStats.Total + debtsStats.Total + depositsIncomes.Total,
                AccountStats = accountStats,
                BrokerAccountStats = brokerAccountStats,
                DebtStats = debtsStats,
                DepositsIncomes = depositsIncomes,
                TransactionStats = await GetTransactionDate()
            };
        }

        private async Task<TransactionStatsDto> GetTransactionDate()
        {
            var userProfile = await _userProfile.Get();

            var currentDate = DateTime.Now;
            var transactions = (await _transactionsService
                .GetAll(currentDate.Month, currentDate.Year))
                //TODO: migrate to db level
                .Where(transaction => !transaction.IsSystem);

            var spents = new Dictionary<string, decimal>();
            var incomes = new Dictionary<string, decimal>();

            decimal spentsTotal = 0;
            decimal incomesTotal = 0;

            foreach (var transaction in transactions) 
            {
                if (transaction.MoneyQuantity > 0)
                {
                    incomesTotal += HandleTransaction(transaction, incomes);
                }
                else
                {
                    spentsTotal += HandleTransaction(transaction, spents);
                }
            }

            return new TransactionStatsDto()
            {
                SpentsTotal = spentsTotal,
                IncomesTotal = incomesTotal,
                SpentsDistribution = GetTransactionsDistribution(userProfile.Currency.Name, spents),
                IncomesDistribution = GetTransactionsDistribution(userProfile.Currency.Name, incomes)
            };
        }

        private IEnumerable<DistributionDto> GetTransactionsDistribution(string currencyName, 
            Dictionary<string, decimal> transactionTypesDistribution)
        {
            return transactionTypesDistribution
                .Select(spent => new DistributionDto()
                {
                    Name = spent.Key,
                    Currency = currencyName,
                    Amount = spent.Value,
                    ConvertedAmount = spent.Value
                });
        }

        private decimal HandleTransaction(TransactionDTO transaction, Dictionary<string, decimal> distribution)
        {
            var typeName = transaction.TransactionType.Name;
            var amount = Math.Abs(transaction.MoneyQuantity);
            var convertedAmount = amount * transaction.Account.Currency.Rate;

            if (distribution.ContainsKey(typeName))
            {
                distribution[typeName] += convertedAmount;
            }
            else
            {
                distribution.Add(typeName, convertedAmount);
            }

            return convertedAmount;
        }

        private async Task<AccountStatsDto> GetAccountData()
        {
            var accounts = await _accountService.GetAll(true);

            var cashValuesDistribution = new List<DistributionDto>();
            var depositsDistribution = new List<DistributionDto>();
            var bankAccountsDistribution = new List<DistributionDto>();

            decimal cashSummary = 0;
            decimal depositSummary = 0;
            decimal bankAccountSummary = 0;

            foreach (var account in accounts)
            {
                switch (account.AccountTypeId) { }

                if (account.AccountTypeId == AccountTypeConstants.Cash)
                {
                    cashSummary += HandleCard(account, cashValuesDistribution);
                }
                else if (account.AccountTypeId == AccountTypeConstants.DepositAccount)
                {
                    depositSummary += HandleCard(account, depositsDistribution);
                }
                else if (account.AccountTypeId == AccountTypeConstants.DebitCard ||
                    account.AccountTypeId == AccountTypeConstants.CreditCard)
                {
                    bankAccountSummary += HandleCard(account, bankAccountsDistribution);
                }
            }

            return new AccountStatsDto()
            {
                Total = cashSummary + depositSummary + bankAccountSummary,

                TotalCash = cashSummary,
                TotalDeposit = depositSummary,
                TotalBankAccount = bankAccountSummary,

                CashDistribution = cashValuesDistribution,
                DepositsDistribution = depositsDistribution,
                BankAccountsDistribution = bankAccountsDistribution
            };
        }

        private decimal HandleCard(AccountDTO account, List<DistributionDto> cardValues)
        {
            var key = account.Name;

            var convertedAmount = account.Balance * account.Currency.Rate;

            cardValues.Add(new DistributionDto()
            {
                Name = account.Name,
                Currency = account.Currency.Name,
                Amount = account.Balance,
                ConvertedAmount = convertedAmount
            });

            return convertedAmount;
        }

        private async Task<BrokerAccountStatsDto> GetBrokerAccountData()
        {
            var brokerAccounts = await _brokerAccountService.GetAll();

            var brokerAccountsValues = new List<DistributionDto>();
            decimal brokerAccountsSummary = 0;

            foreach (var brokerAccount in brokerAccounts)
            {
                var key = brokerAccount.Name;
                var convertedAmount = brokerAccount.CurrentValue * brokerAccount.Currency.Rate;
                brokerAccountsSummary += convertedAmount;

                brokerAccountsValues.Add(new DistributionDto()
                {
                    Name = brokerAccount.Name,
                    Currency = brokerAccount.Currency.Name,
                    Amount = brokerAccount.CurrentValue,
                    ConvertedAmount = convertedAmount
                });
            }

            return new BrokerAccountStatsDto()
            {
                Total = brokerAccountsSummary,
                Distribution = brokerAccountsValues
            };
        }

        private async Task<DebtStatsDto> GetDebtsData()
        {
            var debts = await _debtService.GetAll();

            var debtsDistribution = new List<DistributionDto>();
            decimal debtsSummary = 0;

            foreach (var debt in debts)
            {
                var key = debt.Name;
                var convertedAmount = debt.Amount * debt.Currency.Rate;
                debtsSummary += convertedAmount;

                debtsDistribution.Add(new DistributionDto()
                {
                    Name = debt.Name,
                    Currency = debt.Currency.Name,
                    Amount = debt.Amount,
                    ConvertedAmount = convertedAmount
                });
            }

            return new DebtStatsDto()
            {
                Total = debtsSummary,
                Distribution = debtsDistribution
            };
        }

        private async Task<DepositsIncomesDto> GetDepositsIncomesData()
        {
            var deposits = await _depositService.GetAllActive();

            var debtsDistribution = new List<DistributionDto>();
            decimal debtsSummary = 0;

            foreach (var deposit in deposits)
            {
                var key = deposit.Name;
                var currency = deposit.Account.Currency;
                var totalDays = deposit.To.DayNumber - deposit.From.DayNumber;
                var daysPassed = DateOnly.FromDateTime(DateTime.Now).DayNumber - deposit.From.DayNumber;
                var amount = deposit.EstimatedEarn / totalDays * daysPassed;
                var convertedAmount = amount * currency.Rate;
                debtsSummary += convertedAmount;

                debtsDistribution.Add(new DistributionDto()
                {
                    Name = key,
                    Currency = currency.Name,
                    Amount = amount,
                    ConvertedAmount = convertedAmount
                });
            }

            return new DepositsIncomesDto()
            {
                Total = debtsSummary,
                Distribution = debtsDistribution
            };

        }
    }
}
