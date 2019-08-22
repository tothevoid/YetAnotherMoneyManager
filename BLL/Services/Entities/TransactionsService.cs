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
        private readonly IMapper _mapper;
        public TransactionsService(IUnitOfWork uow, IMapper mapper)
        {
            _db = uow;
            _mapper = mapper;
            _transactionsRepo = uow.CreateTransactionRepository();
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
            transaction.FundSourceId = transactionDTO?.FundSource?.Id ?? default;
            await _transactionsRepo.Add(transaction);
            _db.Commit();
            return transaction.Id;
        }

        public async Task Update(TransactionDTO transactionDTO)
        {
            var transaction = _mapper.Map<Transaction>(transactionDTO);
            await _transactionsRepo.Update(transaction);
            _db.Commit();
        }

        public async Task Delete(Guid id)
        {
            await _transactionsRepo.Delete(id);
            _db.Commit();
        }
    }
}