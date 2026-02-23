using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MoneyManager.Application.DTO.Transactions;
using MoneyManager.Application.Interfaces.Transactions;
using MoneyManager.Infrastructure.Entities.Currencies;
using MoneyManager.Infrastructure.Entities.Transactions;
using MoneyManager.Infrastructure.Interfaces.Database;
using MoneyManager.Infrastructure.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
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
            var query = new ComplexQueryBuilder<CurrencyTransaction>()
                .AddJoins(GetFullHierarchyColumns)
                .AddOrder(CurrencyTransaction => CurrencyTransaction.Date)
                .GetQuery();
            var currencyTransactions = await _currencyTransactionRepo.GetAll(query);
            
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

        public async Task<CurrencyTransactionDto> GetById(Guid id)
        {
            var entity = await _currencyTransactionRepo.GetById(id, include: GetFullHierarchyColumns);
            return _mapper.Map<CurrencyTransactionDto>(entity);
        }

        public async Task<IEnumerable<CurrencyTransactionDto>> GetAllByAccountId(Guid accountId)
        {
            var query = new ComplexQueryBuilder<CurrencyTransaction>()
                .AddFilter(x => x.SourceAccountId == accountId || x.DestinationAccountId == accountId)
                .AddJoins(GetFullHierarchyColumns)
                .AddOrder(CurrencyTransaction => CurrencyTransaction.Date)
                .GetQuery();

            var transactions = await _currencyTransactionRepo.GetAll(query);

            return _mapper.Map<IEnumerable<CurrencyTransactionDto>>(transactions);
        }

        private IQueryable<CurrencyTransaction> GetFullHierarchyColumns(
            IQueryable<CurrencyTransaction> currencyTransactionQuery)
        {
            return currencyTransactionQuery
                .Include(currencyTransaction => currencyTransaction.SourceAccount.Currency)
                .Include(currencyTransaction => currencyTransaction.SourceAccount.AccountType)
                .Include(currencyTransaction => currencyTransaction.SourceAccount.Bank)
                .Include(currencyTransaction => currencyTransaction.DestinationAccount.Currency)
                .Include(currencyTransaction => currencyTransaction.DestinationAccount.AccountType)
                .Include(currencyTransaction => currencyTransaction.DestinationAccount.Bank);
        }
    }
}
