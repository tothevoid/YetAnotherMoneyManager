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

        public async Task<IEnumerable<TransactionDTO>> GetAll()
        {
            var transactions = await _transactionsRepo.GetAllFull();
            return _mapper.Map<IEnumerable<TransactionDTO>>(transactions.OrderByDescending(x => x.Date));
        }

        public async Task<Guid> Add(TransactionDTO transactionDTO)
        {
            var transaction = _mapper.Map<Transaction>(transactionDTO);
            transaction.Id = Guid.NewGuid();
            var sourceId = transactionDTO?.FundSource?.Id ?? default;
            var tasks = new List<Task>();
            if (sourceId != default)
            {
                transaction.FundSourceId = transactionDTO.FundSource.Id;
                tasks.Add(_fundRepo.Increment(sourceId, x => x.Balance, transaction.MoneyQuantity));
            }
            tasks.Add(_transactionsRepo.Add(transaction));
            await Task.WhenAll(tasks);
            _db.Commit();
            return transaction.Id;
        }

        public async Task Update(TransactionDTO transactionDTO)
        {
            var transaction = _mapper.Map<Transaction>(transactionDTO);
            await _transactionsRepo.Update(transaction);
            _db.Commit();
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