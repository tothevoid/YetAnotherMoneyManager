using Microsoft.EntityFrameworkCore;
using MoneyManager.Application.DTO.Brokers;
using MoneyManager.Application.DTO.Common;
using MoneyManager.Application.DTO.Securities;
using MoneyManager.Application.Interfaces.Securities;
using MoneyManager.Application.Mappings;
using MoneyManager.Infrastructure.Entities.Brokers;
using MoneyManager.Infrastructure.Entities.Debts;
using MoneyManager.Infrastructure.Entities.Deposits;
using MoneyManager.Infrastructure.Entities.Securities;
using MoneyManager.Infrastructure.Interfaces.Database;
using MoneyManager.Infrastructure.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MoneyManager.Application.Services.Securities
{
    public class DividendService : IDividendService
    {
        private readonly IUnitOfWork _db;

        private readonly IRepository<Dividend> _dividendRepo;
        private readonly ApplicationMapper _mapper;
        public DividendService(IUnitOfWork uow, ApplicationMapper mapper)
        {
            _db = uow;
            _mapper = mapper;
            _dividendRepo = uow.CreateRepository<Dividend>();
        }

        public async Task<IEnumerable<DividendDto>> GetAllAsync(Guid securityId, int pageIndex, int recordsQuantity)
        {
            var query = new ComplexQueryBuilder<Dividend>()
                .AddPagination(pageIndex, recordsQuantity,
                    (dividend) => dividend.SnapshotDate, true)
                .AddFilter((dividend) => dividend.SecurityId == securityId)
                .AddJoins(GetFullHierarchyColumns)
                .GetQuery();

            var dividends = await _dividendRepo.GetAllAsync(query);
            return _mapper.Map(dividends);
        }

        public async Task<PaginationConfigDto> GetPaginationAsync(Guid securityId)
        {
            int pageSize = 10;
            var recordsQuantity = await _dividendRepo.GetCountAsync(GetBaseFilter(securityId));

            return new PaginationConfigDto()
            {
                PageSize = pageSize,
                RecordsQuantity = recordsQuantity
            };
        }

        private Expression<Func<Dividend, bool>> GetBaseFilter(Guid securityId)
        {
            return brokerAccountSecurity => brokerAccountSecurity.SecurityId == securityId;
        }

        public async Task<IEnumerable<DividendDto>> GetAvailableAsync(Guid brokerAccountId)
        {
            var securities = await _dividendRepo
                .GetAllAsync((dividend) => dividend.DividendPayments.All(payment => 
                       !dividend.DividendPayments.Any(p => p.BrokerAccountId == brokerAccountId) &&
                        dividend.Security.BrokerAccountSecurities.Any(s => s.BrokerAccountId == brokerAccountId)
                    ),
                    include: GetFullHierarchyColumns);
            return _mapper.Map(securities);
        }

        public async Task UpdateAsync(DividendDto securityTypeDto)
        {
            var dividend = _mapper.Map(securityTypeDto);
            _dividendRepo.Update(dividend);
            await _db.CommitAsync();
        }

        public async Task<Guid> AddAsync(DividendDto securityDto)
        {
            var dividend = _mapper.Map(securityDto);
            dividend.Id = Guid.NewGuid();
            await _dividendRepo.AddAsync(dividend);
            await _db.CommitAsync();
            return dividend.Id;
        }

        public async Task DeleteAsync(Guid id)
        {
            await _dividendRepo.DeleteAsync(id);
            await _db.CommitAsync();
        }

        private IQueryable<Dividend> GetFullHierarchyColumns(IQueryable<Dividend> dividendQuery)
        {
            return dividendQuery.Include(dividend => dividend.Security.Currency);
        }
    }
}
