using AutoMapper;
using MoneyManager.BLL.DTO;
using MoneyManager.DAL.Entities;
using MoneyManager.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BLL.DTO;
using MoneyManager.WEB.Model;

namespace MoneyManager.BLL.Services.Entities
{
    public class AccountService : IAccountService
    {
        private readonly IUnitOfWork _db;
        private readonly IAccountRepository _accountRepo;
        private readonly IRepository<Transaction> _transactionRepo;
        private readonly IMapper _mapper;
        public AccountService(IUnitOfWork uow, IMapper mapper)
        {
            _db = uow;
            _mapper = mapper;
            _accountRepo = uow.CreateAccountRepository();
            _transactionRepo = uow.CreateRepository<Transaction>();
        }

        public IEnumerable<AccountDTO> GetAll(bool onlyActive)
        {
            var transactions = onlyActive ?
                _accountRepo.GetAllFull(account => account.Active) :
                _accountRepo.GetAllFull();
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

            var tasks = new List<Task>()
            {
                _accountRepo.Update(account)
            };

            var balanceDiff = account.Balance - currentAccountState.Balance;
            if (Math.Abs(balanceDiff) > 0.0001m)
            {
                var transaction = GenerateSystemTransaction(account,
                    $"{currentAccountState.Balance} => {account.Balance}", balanceDiff);
                tasks.Add(_transactionRepo.Add(transaction));
            }

            await Task.WhenAll(tasks);
            _db.Commit();
        }

        public async Task<Guid> Add(AccountDTO transactionDTO)
        {
            var account = _mapper.Map<Account>(transactionDTO);
            account.Id = Guid.NewGuid();
            await _accountRepo.Add(account);
            _db.Commit();
            return account.Id;
        }

        public async Task Delete(Guid id)
        {
            await _accountRepo.Delete(id);
            _db.Commit();
        }

        public async Task Transfer(AccountTransferDto transferDto)
        {
            var accounts = (await _accountRepo.GetAll(account => 
                account.Id == transferDto.From || account.Id == transferDto.To)).ToList();
            var fromAccount = accounts.FirstOrDefault(account => account.Id == transferDto.From);
            var toAccount = accounts.FirstOrDefault(account => account.Id == transferDto.To);

            if (fromAccount == null || toAccount == null)
            {
                throw new ArgumentException(nameof(transferDto));
            }

            fromAccount.Balance -= (transferDto.Balance + transferDto.Fee);
            toAccount.Balance += transferDto.Balance;

            var fromAccountTransaction = GenerateSystemTransaction(fromAccount, $"-{transferDto.Balance} => {toAccount.Name} (Fee: {transferDto.Fee})", 
                -1 * transferDto.Balance - transferDto.Fee);
            var toAccountTransaction = GenerateSystemTransaction(fromAccount, $"+{transferDto.Balance} <= {fromAccount.Name}", transferDto.Balance);

            var tasks = new List<Task>()
            {
                _accountRepo.Update(fromAccount),
                _accountRepo.Update(toAccount),
                _transactionRepo.Add(fromAccountTransaction),
                _transactionRepo.Add(toAccountTransaction),
            };

            await Task.WhenAll(tasks);
            _db.Commit();
        }

        public async Task<AccountCurrencySummaryDto[]> GetSummary()
        {
            //TODO: Group on db level
            var accounts = await _accountRepo.GetAll(account => account.Active);
            var groups = accounts.GroupBy(account => account.CurrencyId)
                .Select(group => new AccountCurrencySummaryDto()
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
    }
}