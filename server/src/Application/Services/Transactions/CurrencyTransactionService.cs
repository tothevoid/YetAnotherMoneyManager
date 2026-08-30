using Microsoft.EntityFrameworkCore;
using Audex.Application.DTO.Common;
using Audex.Application.DTO.Transactions;
using Audex.Application.Interfaces.Transactions;
using Audex.Application.Mappings;
using Audex.Infrastructure.Entities.Currencies;
using Audex.Infrastructure.Entities.Transactions;
using Audex.Infrastructure.Interfaces.Database;
using Audex.Infrastructure.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Audex.Application.Services.Transactions
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
                .AddOrder(CurrencyTransaction => CurrencyTransaction.Date, isDescending: true)
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

        public async Task<IEnumerable<CurrencyTransactionDto>> GetAllByAccountIdAsync(Guid accountId, int? pageIndex = null, int? recordsQuantity = null)
        {
            var builder = new ComplexQueryBuilder<CurrencyTransaction>()
                .AddFilter(x => x.SourceAccountId == accountId || x.DestinationAccountId == accountId)
                .AddJoins(GetFullHierarchyColumns);

            if (pageIndex.HasValue && recordsQuantity.HasValue && recordsQuantity.Value > 0)
            {
                builder.AddPagination(pageIndex.Value, recordsQuantity.Value, x => x.Date, true);
            }
            else
            {
                builder.AddOrder(x => x.Date, isDescending: true);
            }

            var transactions = await _currencyTransactionRepo.GetAllAsync(builder.GetQuery());

            return _mapper.Map(transactions);
        }

        public async Task<PaginationConfigDto> GetPaginationAsync(Guid accountId)
        {
            const int pageSize = 10;
            var recordsQuantity = await _currencyTransactionRepo.GetCountAsync(
                x => x.SourceAccountId == accountId || x.DestinationAccountId == accountId);

            return new PaginationConfigDto
            {
                PageSize = pageSize,
                RecordsQuantity = recordsQuantity
            };
        }

        public async Task<CurrencyAccountSummaryDto> GetSummaryByAccountIdAsync(Guid accountId)
        {
            var query = new ComplexQueryBuilder<CurrencyTransaction>()
                .AddFilter(x => x.SourceAccountId == accountId || x.DestinationAccountId == accountId)
                .AddJoins(GetFullHierarchyColumns)
                .GetQuery();

            var transactions = (await _currencyTransactionRepo.GetAllAsync(query)).ToList();

            decimal totalPnl = 0;
            foreach (var transaction in transactions)
            {
                var currentRate = tr.DestinationAccount?.Currency?.Rate ?? 0;
                var pnl = (currentRate - transaction.Rate) * tr.Amount;
                totalPnl += pnl;
            }

            return new CurrencyAccountSummaryDto
            {
                TotalPnl = totalPnl,
                TransactionsCount = transactions.Count
            };
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
