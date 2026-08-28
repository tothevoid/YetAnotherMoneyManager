using Microsoft.EntityFrameworkCore;
using MoneyManager.Application.DTO.Transactions;
using MoneyManager.Application.Interfaces.Transactions;
using MoneyManager.Application.Mappings;
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
        private readonly ApplicationMapper _mapper;

        public CurrencyTransactionService(IUnitOfWork uow, ApplicationMapper mapper)
        {
            _db = uow;
            _mapper = mapper;
            _currencyTransactionRepo = uow.CreateRepository<CurrencyTransaction>();
        }

        public async Task<IEnumerable<CurrencyTransactionDto>> GetAllAsync()
        {
            var query = new ComplexQueryBuilder<CurrencyTransaction>()
                .AddJoins(GetFullHierarchyColumns)
                .AddOrder(CurrencyTransaction => CurrencyTransaction.Date)
                .GetQuery();
            var currencyTransactions = await _currencyTransactionRepo.GetAllAsync(query);
            
            return _mapper.Map(currencyTransactions);
        }

        public async Task UpdateAsync(CurrencyTransactionDto currencyTransactionDto)
        {
            var currencyTransaction = _mapper.Map(currencyTransactionDto);
            _currencyTransactionRepo.Update(currencyTransaction);
            await _db.CommitAsync();
        }

        public async Task<Guid> AddAsync(CurrencyTransactionDto currencyTransactionDto)
        {
            var currencyTransaction = _mapper.Map(currencyTransactionDto);
            currencyTransaction.Id = Guid.NewGuid();
            await _currencyTransactionRepo.AddAsync(currencyTransaction);
            await _db.CommitAsync();
            return currencyTransaction.Id;
        }

        public async Task DeleteAsync(Guid id)
        {
            await _currencyTransactionRepo.DeleteAsync(id);
            await _db.CommitAsync();
        }

        public async Task<CurrencyTransactionDto> GetByIdAsync(Guid id)
        {
            var entity = await _currencyTransactionRepo.GetByIdAsync(id, include: GetFullHierarchyColumns);
            return _mapper.Map(entity);
        }

        public async Task<IEnumerable<CurrencyTransactionDto>> GetAllByAccountIdAsync(Guid accountId)
        {
            var query = new ComplexQueryBuilder<CurrencyTransaction>()
                .AddFilter(x => x.SourceAccountId == accountId || x.DestinationAccountId == accountId)
                .AddJoins(GetFullHierarchyColumns)
                .AddOrder(CurrencyTransaction => CurrencyTransaction.Date)
                .GetQuery();

            var transactions = await _currencyTransactionRepo.GetAllAsync(query);

            return _mapper.Map(transactions);
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
