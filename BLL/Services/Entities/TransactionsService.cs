using AutoMapper;
using MoneyManager.BLL.DTO;
using MoneyManager.DAL.Entities;
using MoneyManager.DAL.Interfaces;
using System;
using System.Collections.Generic;
using MoneyManager.Common;
using System.Threading.Tasks;

namespace MoneyManager.BLL.Services.Entities
{
    public class TransactionsService: ITransactionsService
    {
        private readonly IUnitOfWork _db;
        private readonly IRepository<Transaction> _transactionsRepo;
        private readonly IMapper _mapper;
        public TransactionsService(IUnitOfWork uow, IMapper mapper)
        {
            _db = uow;
            _mapper = mapper;
            _transactionsRepo = uow.CreateRepository<Transaction>();
        }

        public async Task<IEnumerable<TransactionDTO>> GetAll()
        {
            var transactions = await _transactionsRepo.GetAll();
            return _mapper.Map<IEnumerable<TransactionDTO>>(transactions);
        }

        public async Task<Guid> Add(TransactionDTO transactionDTO)
        {
            var transaction = _mapper.Map<Transaction>(transactionDTO);
            transaction.Id = Guid.NewGuid();
            await _transactionsRepo.Add(transaction);
            _db.Commit();
            return transaction.Id;
        }

        public async Task Delete(Guid id)
        {
            await _transactionsRepo.Delete(id);
            _db.Commit();
        }
    }
}