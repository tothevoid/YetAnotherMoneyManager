using AutoMapper;
using MoneyManager.Application.DTO.Transactions;
using MoneyManager.Application.Interfaces.Transactions;
using MoneyManager.Infrastructure.Entities.Transactions;
using MoneyManager.Infrastructure.Interfaces.Database;
using MoneyManager.Infrastructure.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MoneyManager.Infrastructure.Entities.Accounts;
using MoneyManager.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

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

        public async Task<IEnumerable<TransactionDTO>> GetAll(int month, int year)
        {
            var (startDate, endDate) = GetDateRange(month, year);

            var transactions = await _transactionsRepo.GetAll(transaction => 
                transaction.Date >= startDate && transaction.Date <= endDate);
            return _mapper.Map<IEnumerable<TransactionDTO>>(transactions.OrderByDescending(x => x.Date));
        }

        public async Task<Guid> Add(TransactionDTO transactionDTO)
        {
            var transaction = _mapper.Map<Transaction>(transactionDTO);
            transaction.Id = Guid.NewGuid();
            var sourceId = transactionDTO?.Account?.Id ?? default;
            //var transactionTypeId = transactionDTO?.TransactionType?.Id ?? default;
            var tasks = new List<Task>();
            if (sourceId != default)
            {
                transaction.AccountId = sourceId;
                tasks.Add(_accountRepo.Increment(sourceId, x => x.Balance, transaction.MoneyQuantity));
            }
            //if (transactionTypeId != default)
            //{
            //    transaction.TransactionTypeId = transactionTypeId;
            //}
            tasks.Add(_transactionsRepo.Add(transaction));
            await Task.WhenAll(tasks);
            await _db.Commit();
            return transaction.Id;
        }

        public async Task<List<UpdateAccountDTO>> Update(TransactionDTO updateTransactionModel)
        {
            var transaction = _mapper.Map<Transaction>(updateTransactionModel);
            var sourceId = updateTransactionModel?.Account?.Id ?? default;
            if (sourceId != default)
            {
                transaction.AccountId = updateTransactionModel.Account.Id;
            }

            //TODO: Fix duplication and make it automatic
            //var transactionTypeId = transaction?.TransactionType?.Id ?? default;
            //if (transactionTypeId != default)
            //{
            //    transaction.TransactionTypeId = transactionTypeId;
            //}

            _transactionsRepo.Update(transaction);
            var lastTransaction = await _transactionsRepo.GetById(updateTransactionModel.Id);
            var lastTransactionDTO = _mapper.Map<TransactionDTO>(lastTransaction);

            var accountsToUpdate = await RecalculateAccount(lastTransactionDTO, updateTransactionModel);
            await _db.Commit();
            return accountsToUpdate;
        }

        private async Task<List<UpdateAccountDTO>> RecalculateAccount( 
            TransactionDTO lastTransaction, TransactionDTO updateAccountModel)
        {
            var accountsToUpdate = new List<UpdateAccountDTO>();
            var tasks = new List<Task>() { };
            var lastTransactionId = lastTransaction?.Account?.Id ?? default;
            var updateAccountModelId = updateAccountModel?.Account?.Id ?? default;
            //account deleted from transaction
            if (lastTransactionId != default && updateAccountModelId == default)
            {
                var difference = lastTransaction.MoneyQuantity * -1;
                accountsToUpdate.Add(new UpdateAccountDTO() { AccountId = lastTransactionId, Delta = difference });
            }
            //account added to transaction
            else if (lastTransactionId == default && updateAccountModelId != default)
            {
                accountsToUpdate.Add(new UpdateAccountDTO() { AccountId = updateAccountModelId, Delta = updateAccountModel.MoneyQuantity });
            }
            //changed account from transaction
            else if (lastTransactionId != default && updateAccountModelId != default &&
                lastTransaction.Account.Id != updateAccountModel.Account.Id)
            {
                accountsToUpdate.Add(new UpdateAccountDTO() { AccountId = lastTransactionId, Delta = lastTransaction.MoneyQuantity * -1 });
                accountsToUpdate.Add(new UpdateAccountDTO() { AccountId = updateAccountModelId, Delta = updateAccountModel.MoneyQuantity });
            }
            //changed money quantity of the same account
            else if (lastTransactionId != default && updateAccountModelId != default &&
                lastTransaction.MoneyQuantity != updateAccountModel.MoneyQuantity)
            {
                var difference = updateAccountModel.MoneyQuantity - lastTransaction.MoneyQuantity;
                accountsToUpdate.Add(new UpdateAccountDTO() { AccountId = updateAccountModelId, Delta = difference });
            }
            foreach (var account in accountsToUpdate)
            {
                tasks.Add(_accountRepo.Increment(account.AccountId, x => x.Balance, account.Delta));
            }
            await Task.WhenAll(tasks);
            return accountsToUpdate;
        }

        public async Task Delete(Guid id)
        {
            var transaction = await _transactionsRepo.GetById(id);

            if (transaction == null)
            {
                throw new ArgumentException(nameof(id));
            }

            var tasks = new List<Task>();
            var sourceId = transaction?.Account?.Id ?? default;
            if (sourceId != default && transaction.MoneyQuantity != 0)
            {
                tasks.Add(_accountRepo.Increment(sourceId, x => x.Balance, transaction.MoneyQuantity * -1));
            }
            tasks.Add(_transactionsRepo.Delete(transaction.Id));
            
            await Task.WhenAll(tasks);
            await _db.Commit();
        }

        private (DateOnly, DateOnly) GetDateRange(int month, int year)
        {
            var startDate = new DateOnly(year, month, 1);
            var endDate = new DateOnly(year, month, 1).AddMonths(1).AddDays(-1);
            return (startDate, endDate);
        }
    }
}