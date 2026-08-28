using MoneyManager.Application.DTO.Transactions;
using MoneyManager.Application.Interfaces.Transactions;
using MoneyManager.Application.Mappings;
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
        private readonly ApplicationMapper _mapper;
        public TransactionsService(IUnitOfWork uow, ApplicationMapper mapper)
        {
            _db = uow;
            _mapper = mapper;
            _transactionsRepo = uow.CreateRepository<Transaction>();
            _accountRepo = uow.CreateRepository<Account>();
        }

        public async Task<TransactionDto> GetByIdAsync(Guid id)
        {
            var transaction =  await _transactionsRepo.GetByIdAsync(id);
            return _mapper.Map(transaction);
        }

        public async Task<IEnumerable<TransactionDto>> GetAllAsync(int month, int year, bool showSystem)
        {
            var (startDate, endDate) = GetDateRange(month, year);

            var transactions = await _transactionsRepo.GetAllAsync(transaction => 
                transaction.Date >= startDate && transaction.Date <= endDate && (showSystem || !transaction.IsSystem),
                GetFullHierarchyColumns);
            return _mapper.Map(transactions.OrderByDescending(x => x.Date));
        }

        public async Task<TransactionDto> AddAsync(TransactionDto transactionDto)
        {
            var transaction = _mapper.Map(transactionDto);
            transaction.Id = Guid.NewGuid();
            var sourceId = transactionDto.AccountId != Guid.Empty ? transactionDto.AccountId : (transactionDto?.Account?.Id ?? default);
            if (sourceId != default)
            {
                transaction.AccountId = sourceId;

                var account = await _accountRepo.GetByIdAsync(sourceId);
                account.Balance += transaction.Amount;
                _accountRepo.Update(account);
            }

            await _transactionsRepo.AddAsync(transaction);
            await _db.CommitAsync();

            var newTransaction = await _transactionsRepo.GetByIdAsync(transaction.Id, GetFullHierarchyColumns, true);
            return _mapper.Map(newTransaction);
        }

        public async Task UpdateAsync(TransactionDto transactionToUpdate)
        {
            var transaction = _mapper.Map(transactionToUpdate);
            var sourceId = transactionToUpdate.AccountId != Guid.Empty ? transactionToUpdate.AccountId : (transactionToUpdate?.Account?.Id ?? default);
            if (sourceId != default)
            {
                transaction.AccountId = sourceId;
            }

            var lastTransaction = await _transactionsRepo.GetByIdAsync(transactionToUpdate.Id);
            var lastTransactionDto = _mapper.Map(lastTransaction);
            _transactionsRepo.Update(transaction);

            await RecalculateAccount(lastTransactionDto, transactionToUpdate);
            await _db.CommitAsync();
        }

        private async Task RecalculateAccount(TransactionDto currentTransaction, TransactionDto updatedTransaction)
        {
            var accountsToUpdate = new List<(Guid accountId, decimal delta)>();
            var lastTransactionId = currentTransaction.AccountId != Guid.Empty ? currentTransaction.AccountId : (currentTransaction?.Account?.Id ?? default);
            var updateAccountModelId = updatedTransaction.AccountId != Guid.Empty ? updatedTransaction.AccountId : (updatedTransaction?.Account?.Id ?? default);

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
                lastTransactionId != updateAccountModelId)
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
                var accountEntity = await _accountRepo.GetByIdAsync(account.accountId);
                accountEntity.Balance += account.delta;
                _accountRepo.Update(accountEntity);
            }
        }

        public async Task DeleteAsync(Guid id)
        {
            var transaction = await _transactionsRepo.GetByIdAsync(id);

            if (transaction == null)
            {
                throw new ArgumentException(nameof(id));
            }

            var sourceId = transaction.AccountId != Guid.Empty ? transaction.AccountId : (transaction?.Account?.Id ?? default);
            if (sourceId != default && transaction.Amount != 0)
            {
                var accountEntity = await _accountRepo.GetByIdAsync(sourceId);
                accountEntity.Balance += transaction.Amount * -1;
                _accountRepo.Update(accountEntity);

            }

            await _transactionsRepo.DeleteAsync(transaction.Id);
            await _db.CommitAsync();
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
                .Include(transaction => transaction.Account.AccountType)
                .Include(transaction => transaction.Account.Bank);
        }
    }
}