using AutoMapper;
using MoneyManager.Application.DTO.Transactions;
using MoneyManager.Application.Interfaces.Transactions;
using MoneyManager.Infrastructure.Entities.Transactions;
using MoneyManager.Infrastructure.Interfaces.Database;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MoneyManager.Application.Services.Transactions
{
    public class CurrencyTransactionService: ICurrencyTransactionService
    {
        private readonly IUnitOfWork _db;
        private readonly IRepository<CurrencyTransaction> _currencyTransactionRepo;
        private readonly IMapper _mapper;

        public CurrencyTransactionService(IUnitOfWork uow, IMapper mapper)
        {
            _db = uow;
            _mapper = mapper;
            _currencyTransactionRepo = uow.CreateRepository<CurrencyTransaction>();
        }

        public async Task<IEnumerable<CurrencyTransactionDto>> GetAll()
        {
            var currencyTransactions = await _currencyTransactionRepo.GetAll();
            return _mapper.Map<IEnumerable<CurrencyTransactionDto>>(currencyTransactions);
        }

        public async Task Update(CurrencyTransactionDto currencyTransactionDto)
        {
            var currencyTransaction = _mapper.Map<CurrencyTransaction>(currencyTransactionDto);
            _currencyTransactionRepo.Update(currencyTransaction);
            await _db.Commit();
        }

        public async Task<Guid> Add(CurrencyTransactionDto currencyTransactionDto)
        {
            var currencyTransaction = _mapper.Map<CurrencyTransaction>(currencyTransactionDto);
            currencyTransaction.Id = Guid.NewGuid();
            await _currencyTransactionRepo.Add(currencyTransaction);
            await _db.Commit();
            return currencyTransaction.Id;
        }

        public async Task Delete(Guid id)
        {
            await _currencyTransactionRepo.Delete(id);
            await _db.Commit();
        }
    }
}
