using AutoMapper;
using MoneyManager.Application.DTO.Transactions;
using MoneyManager.Application.Interfaces.Transactions;
using MoneyManager.Infrastructure.Entities.Transactions;
using MoneyManager.Infrastructure.Interfaces.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MoneyManager.Infrastructure.Entities.Accounts;
using MoneyManager.Infrastructure.Entities.Securities;

namespace MoneyManager.Application.Services.Transactions
{
    public class TransactionsService: ITransactionsService
    {
        private readonly IUnitOfWork _db;
        private readonly IRepository<Transaction> _transactionsRepo;
        private readonly IRepository<Account> _accountRepo;
        private readonly IMapper _mapper;
        public TransactionsService(IUnitOfWork uow, IMapper mapper)
        {
            _db = uow;
            _mapper = mapper;
            _transactionsRepo = uow.CreateRepository<Transaction>();
            _accountRepo = uow.CreateRepository<Account>();
        }

        public async Task<TransactionDTO> GetById(Guid id)
        {
            var transaction =  await _transactionsRepo.GetById(id);
            return _mapper.Map<TransactionDTO>(transaction);
        }

        public async Task<IEnumerable<TransactionDTO>> GetAll(int month, int year, bool showSystem)
        {
            var (startDate, endDate) = GetDateRange(month, year);

            var transactions = await _transactionsRepo.GetAll(transaction => 
                transaction.Date >= startDate && transaction.Date <= endDate && (showSystem || !transaction.IsSystem),
                GetFullHierarchyColumns);
            return _mapper.Map<IEnumerable<TransactionDTO>>(transactions.OrderByDescending(x => x.Date));
        }

        public async Task<TransactionDTO> Add(TransactionDTO transactionDTO)
        {
            var transaction = _mapper.Map<Transaction>(transactionDTO);
            transaction.Id = Guid.NewGuid();
            var sourceId = transactionDTO?.Account?.Id ?? default;
            var tasks = new List<Task>();
            if (sourceId != default)
            {
                transaction.AccountId = sourceId;

                var account = await _accountRepo.GetById(sourceId);
                account.Balance += transaction.Amount;
                _accountRepo.Update(account);
            }
           
            tasks.Add(_transactionsRepo.Add(transaction));
            await Task.WhenAll(tasks);
            await _db.Commit();

            var newTransaction = await _transactionsRepo.GetById(transaction.Id, GetFullHierarchyColumns, true);
            return _mapper.Map<TransactionDTO>(newTransaction);
        }

        public async Task Update(TransactionDTO transactionToUpdate)
        {
            var transaction = _mapper.Map<Transaction>(transactionToUpdate);
            var sourceId = transactionToUpdate?.Account?.Id ?? default;
            if (sourceId != default)
            {
                transaction.AccountId = transactionToUpdate.Account.Id;
            }

            var lastTransaction = await _transactionsRepo.GetById(transactionToUpdate.Id);
            var lastTransactionDto = _mapper.Map<TransactionDTO>(lastTransaction);
            _transactionsRepo.Update(transaction);

            await RecalculateAccount(lastTransactionDto, transactionToUpdate);
            await _db.Commit();
        }

        private async Task RecalculateAccount(TransactionDTO currentTransaction, TransactionDTO updatedTransaction)
        {
            var accountsToUpdate = new List<(Guid accountId, decimal delta)>();
            var lastTransactionId = currentTransaction?.Account?.Id ?? default;
            var updateAccountModelId = updatedTransaction?.Account?.Id ?? default;

            //account deleted from transaction
            if (lastTransactionId != default && updateAccountModelId == default)
            {
                var difference = currentTransaction.Amount * -1;
                accountsToUpdate.Add((lastTransactionId, difference));
            }
            //account added to transaction
            else if (lastTransactionId == default && updateAccountModelId != default)
            {
                accountsToUpdate.Add((updateAccountModelId,updatedTransaction.Amount));
            }
            //changed account from transaction
            else if (lastTransactionId != default && updateAccountModelId != default &&
                currentTransaction.Account.Id != updatedTransaction.Account.Id)
            {
                accountsToUpdate.Add((lastTransactionId, currentTransaction.Amount * -1));
                accountsToUpdate.Add((updateAccountModelId, updatedTransaction.Amount));
            }
            //changed money quantity of the same account
            else if (lastTransactionId != default && updateAccountModelId != default &&
                currentTransaction.Amount != updatedTransaction.Amount)
            {
                var difference = updatedTransaction.Amount - currentTransaction.Amount;
                accountsToUpdate.Add((updateAccountModelId, difference));
            }

            foreach (var account in accountsToUpdate)
            {
                var accountEntity = await _accountRepo.GetById(account.accountId);
                accountEntity.Balance += account.delta;
                _accountRepo.Update(accountEntity);
            }
        }

        public async Task Delete(Guid id)
        {
            var transaction = await _transactionsRepo.GetById(id);

            if (transaction == null)
            {
                throw new ArgumentException(nameof(id));
            }

            var sourceId = transaction?.Account?.Id ?? default;
            if (sourceId != default && transaction.Amount != 0)
            {
                var accountEntity = await _accountRepo.GetById(sourceId);
                accountEntity.Balance += transaction.Amount * -1;
                _accountRepo.Update(accountEntity);

            }

            await _transactionsRepo.Delete(transaction.Id);
            await _db.Commit();
        }

        private (DateOnly, DateOnly) GetDateRange(int month, int year)
        {
            var startDate = new DateOnly(year, month, 1);
            var endDate = new DateOnly(year, month, 1).AddMonths(1).AddDays(-1);
            return (startDate, endDate);
        }

        private IQueryable<Transaction> GetFullHierarchyColumns(
            IQueryable<Transaction> transactionQuery)
        {
            return transactionQuery
                .Include(transaction => transaction.TransactionType)
                .Include(transaction => transaction.Account.Currency)
                .Include(transaction => transaction.Account.AccountType);
        }
    }
}