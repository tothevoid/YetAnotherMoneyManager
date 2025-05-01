using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MoneyManager.Application.DTO.Accounts;
using MoneyManager.Application.Interfaces.Accounts;
using MoneyManager.Infrastructure.Interfaces.Database;
using MoneyManager.Infrastructure.Entities.Transactions;
using MoneyManager.Infrastructure.Entities.Accounts;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace MoneyManager.Application.Services.Accounts
{
    public class AccountService : IAccountService
    {
        private readonly IUnitOfWork _db;
        private readonly IRepository<Account> _accountRepo;
        private readonly IRepository<Transaction> _transactionRepo;
        private readonly IMapper _mapper;
        public AccountService(IUnitOfWork uow, IMapper mapper)
        {
            _db = uow;
            _mapper = mapper;
            _accountRepo = uow.CreateRepository<Account>();
            _transactionRepo = uow.CreateRepository<Transaction>();
        }

        public async Task<IEnumerable<AccountDTO>> GetAll(bool onlyActive)
        {
            Expression<Func<Account, bool>> filter = onlyActive ? 
                account => account.Active : 
                null;

            var transactions = await _accountRepo.GetAll(filter, GetFullHierarchyColumns);
            return _mapper.Map<IEnumerable<AccountDTO>>(transactions);
        }

        public async Task Update(AccountDTO accountDTO)
        {
            var account = _mapper.Map<Account>(accountDTO);

            var currentAccountState = await _accountRepo.GetById(account.Id);
            if (currentAccountState == null)
            {
                return;
            }

            _accountRepo.Update(account);

            var balanceDiff = account.Balance - currentAccountState.Balance;
            if (Math.Abs(balanceDiff) > 0.0001m)
            {
                var transaction = GenerateSystemTransaction(account,
                    $"{currentAccountState.Balance} => {account.Balance}", balanceDiff);
                await _transactionRepo.Add(transaction);
            }

            await _db.Commit();
        }

        public async Task<Guid> Add(AccountDTO accountDto)
        {
            var account = _mapper.Map<Account>(accountDto);
            account.Id = Guid.NewGuid();
            await _accountRepo.Add(account);
            await _db.Commit();
            return account.Id;
        }

        public async Task Delete(Guid id)
        {
            await _accountRepo.Delete(id);
            await _db.Commit();
        }

        public async Task Transfer(AccountTransferDTO transferDto)
        {
            var accounts = (await _accountRepo.GetAll(account => 
                account.Id == transferDto.From || account.Id == transferDto.To)).ToList();
            var fromAccount = accounts.FirstOrDefault(account => account.Id == transferDto.From);
            var toAccount = accounts.FirstOrDefault(account => account.Id == transferDto.To);

            if (fromAccount == null || toAccount == null)
            {
                throw new ArgumentException(nameof(transferDto));
            }

            fromAccount.Balance -= transferDto.Balance + transferDto.Fee;
            toAccount.Balance += transferDto.Balance;

            var fromAccountTransaction = GenerateSystemTransaction(fromAccount, $"-{transferDto.Balance} => {toAccount.Name} (Fee: {transferDto.Fee})", 
                -1 * transferDto.Balance - transferDto.Fee);
            var toAccountTransaction = GenerateSystemTransaction(fromAccount, $"+{transferDto.Balance} <= {fromAccount.Name}", transferDto.Balance);

            _accountRepo.Update(fromAccount);
            _accountRepo.Update(toAccount);

            var tasks = new List<Task>()
            {
                _transactionRepo.Add(fromAccountTransaction),
                _transactionRepo.Add(toAccountTransaction),
            };

            await Task.WhenAll(tasks);
            await _db.Commit();
        }

        public async Task<AccountCurrencySummaryDTO[]> GetSummary()
        {
            //TODO: Group on db level
            var accounts = await _accountRepo.GetAll(account => account.Active);
            var groups = accounts.GroupBy(account => account.CurrencyId)
                .Select(group => new AccountCurrencySummaryDTO()
                {
                    Name = group.First().Currency.Name, 
                    Summary = group.Sum(account => account.Balance)
                });

            return groups.ToArray();
        }

        private Transaction GenerateSystemTransaction(Account account, string title, decimal balance)
        {
            return new Transaction()
            {
                Account = account,
                AccountId = account.Id,
                Date = DateOnly.FromDateTime(DateTime.UtcNow),
                Name = title,
                TransactionType = "System",
                MoneyQuantity = balance,
                IsSystem = true
            };
        }

        private IQueryable<Account> GetFullHierarchyColumns(IQueryable<Account> accountQuery)
        {
            return accountQuery.Include(account => account.Currency)
                .Include(account => account.AccountType);
        }
    }
}