using AutoMapper;
using MoneyManager.BLL.DTO;
using MoneyManager.DAL.Entities;
using MoneyManager.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoneyManager.BLL.Services.Entities
{
    public class TransactionsService: ITransactionsService
    {
        private readonly IUnitOfWork _db;
        private readonly ITransactionRepository _transactionsRepo;
        private readonly IRepository<Fund> _fundRepo;
        private readonly IMapper _mapper;
        public TransactionsService(IUnitOfWork uow, IMapper mapper)
        {
            _db = uow;
            _mapper = mapper;
            _transactionsRepo = uow.CreateTransactionRepository();
            _fundRepo = uow.CreateRepository<Fund>();
        }

        public async Task<IEnumerable<TransactionDTO>> GetAll(int month, int year)
        {
            var transactions = await _transactionsRepo.GetAllFull(month, year);
            return _mapper.Map<IEnumerable<TransactionDTO>>(transactions.OrderByDescending(x => x.Date));
        }

        public async Task<Guid> Add(TransactionDTO transactionDTO)
        {
            var transaction = _mapper.Map<Transaction>(transactionDTO);
            transaction.Id = Guid.NewGuid();
            var sourceId = transactionDTO?.FundSource?.Id ?? default;
            var transactionTypeId = transactionDTO?.TransactionType?.Id ?? default;
            var tasks = new List<Task>();
            if (sourceId != default)
            {
                transaction.FundSourceId = sourceId;
                tasks.Add(_fundRepo.Increment(sourceId, x => x.Balance, transaction.MoneyQuantity));
            }
            if (transactionTypeId != default)
            {
                transaction.TransactionTypeId = transactionTypeId;
            }
            tasks.Add(_transactionsRepo.Add(transaction));
            await Task.WhenAll(tasks);
            _db.Commit();
            return transaction.Id;
        }

        public async Task<List<UpdateFundDTO>> Update(TransactionDTO lastTransaction, TransactionDTO updateFundModel)
        {
            var transaction = _mapper.Map<Transaction>(updateFundModel);
            var sourceId = updateFundModel?.FundSource?.Id ?? default;
            if (sourceId != default)
            {
                transaction.FundSourceId = updateFundModel.FundSource.Id;
            }
            var task = _transactionsRepo.Update(transaction);
            var fundsToUpdate = await RecalculateFund(task, lastTransaction, updateFundModel);
            _db.Commit();
            return fundsToUpdate;
        }

        private async Task<List<UpdateFundDTO>> RecalculateFund(Task transactionTask, 
            TransactionDTO lastTransaction, TransactionDTO updateFundModel)
        {
            var fundToUpdate = new List<UpdateFundDTO>();
            var tasks = new List<Task>() { transactionTask };
            var lastTransactionId = lastTransaction?.FundSource?.Id ?? default;
            var updateFundModelId = updateFundModel?.FundSource?.Id ?? default;
            //fund deleted from transaction
            if (lastTransactionId != default && updateFundModelId == default)
            {
                var difference = lastTransaction.MoneyQuantity * -1;
                fundToUpdate.Add(new UpdateFundDTO() { FundId = lastTransactionId, Delta = difference });
            }
            //fund added to transaction
            else if (lastTransactionId == default && updateFundModelId != default)
            {
                fundToUpdate.Add(new UpdateFundDTO() { FundId = updateFundModelId, Delta = updateFundModel.MoneyQuantity });
            }
            //changed fund from transaction
            else if (lastTransactionId != default && updateFundModelId != default &&
                lastTransaction.FundSource.Id != updateFundModel.FundSource.Id)
            {
                fundToUpdate.Add(new UpdateFundDTO() { FundId = lastTransactionId, Delta = lastTransaction.MoneyQuantity * -1 });
                fundToUpdate.Add(new UpdateFundDTO() { FundId = updateFundModelId, Delta = updateFundModel.MoneyQuantity });
            }
            //changed money quantity of the same fund
            else if (lastTransactionId != default && updateFundModelId != default &&
                lastTransaction.MoneyQuantity != updateFundModel.MoneyQuantity)
            {
                var difference = updateFundModel.MoneyQuantity - lastTransaction.MoneyQuantity;
                fundToUpdate.Add(new UpdateFundDTO() { FundId = updateFundModelId, Delta = difference });
            }
            foreach (var fund in fundToUpdate)
            {
                tasks.Add(_fundRepo.Increment(fund.FundId, x => x.Balance, fund.Delta));
            }
            await Task.WhenAll(tasks);
            return fundToUpdate;
        }

        public async Task Delete(TransactionDTO transactionDTO)
        {
            var tasks = new List<Task>();
            var sourceId = transactionDTO?.FundSource?.Id ?? default;
            if (sourceId != default && transactionDTO.MoneyQuantity != 0)
            {
                tasks.Add(_fundRepo.Increment(sourceId, x => x.Balance, transactionDTO.MoneyQuantity * -1));
            }
            tasks.Add(_transactionsRepo.Delete(transactionDTO.Id));
            await Task.WhenAll(tasks);
            _db.Commit();
        }
    }
}