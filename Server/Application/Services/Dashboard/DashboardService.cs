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
using MoneyManager.Application.Interfaces.Brokers;
using System.Transactions;
using MoneyManager.Infrastructure.Entities.Accounts;
using System.Security.Principal;
using MoneyManager.Application.DTO.Transactions;

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

        public DashboardService(IUnitOfWork uow, IMapper mapper, IAccountService accountService, 
            IDepositService depositService, ITransactionsService transactionsService,
            IBrokerAccountService brokerAccountService)
        {
            _db = uow;
            _mapper = mapper;
            _accountService = accountService;
            _depositService = depositService;
            _transactionsService = transactionsService;
            _brokerAccountService = brokerAccountService;
        }

        public async Task<DashboardDto> GetDashboard()
        {
            var accountStats = await GetAccountData();
            var transactionStats = await GetTransactionDate();
            var brokerAccountStats = await GetBrokerAccountData();

            return new DashboardDto()
            {
                Total = accountStats.Total + brokerAccountStats.Total,
                AccountStats = await GetAccountData(),
                TransactionStats = await GetTransactionDate(),
                BrokerAccountStats = await GetBrokerAccountData()
            };
        }

        private async Task<TransactionStatsDto> GetTransactionDate()
        {
            var currentDate = DateTime.Now;
            var transactions = await _transactionsService.GetAll(currentDate.Month, currentDate.Year);

            var spents = new List<DistributionDto>();
            var incomes = new List<DistributionDto>();

            decimal spentsTotal = 0;
            decimal incomesTotal = 0;

            foreach (var transaction in transactions) 
            {
              
                if (transaction.MoneyQuantity > 0)
                {
                    spentsTotal += HandleTransaction(transaction, spents);
                }
                else
                {
                    incomesTotal += HandleTransaction(transaction, incomes);
                }
            }

            return new TransactionStatsDto()
            {
                SpentsTotal = spentsTotal,
                IncomesTotal = incomesTotal,
                SpentsDistribution = spents,
                IncomesDistribution = incomes
            };
        }

        private decimal HandleTransaction(TransactionDTO transaction, List<DistributionDto> distribution)
        {
            var typeName = transaction.TransactionType.Name;
            var amount = Math.Abs(transaction.MoneyQuantity);
            var convertedAmount = amount * transaction.Account.Currency.Rate;
            
            distribution.Add(new DistributionDto()
            {
                Name = typeName,
                Currency = transaction.Account.Currency.Name,
                Amount = transaction.Account.Balance,
                ConvertedAmount = convertedAmount
            });

            return convertedAmount;
        }

        private async Task<AccountStatsDto> GetAccountData()
        {
            var accounts = await _accountService.GetAll(true);
            var deposits = await _depositService.GetAllActive();

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
    }
}
