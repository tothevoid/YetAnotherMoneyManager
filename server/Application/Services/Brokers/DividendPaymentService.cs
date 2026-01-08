using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MoneyManager.Application.DTO.Brokers;
using MoneyManager.Application.DTO.Common;
using MoneyManager.Application.Interfaces.Brokers;
using MoneyManager.Application.Queries.Brokers;
using MoneyManager.Infrastructure.Entities.Brokers;
using MoneyManager.Infrastructure.Entities.Securities;
using MoneyManager.Infrastructure.Interfaces.Database;
using MoneyManager.Infrastructure.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MoneyManager.Application.Services.Brokers
{
    public class DividendPaymentService : IDividendPaymentService
    {
        private readonly IUnitOfWork _db;

        private readonly IRepository<DividendPayment> _dividendPaymentRepo;
        private readonly IRepository<BrokerAccount> _brokerAccountRepo;
        private readonly IRepository<Dividend> _dividendRepo;
        private readonly IMapper _mapper;

        public DividendPaymentService(IUnitOfWork uow, IMapper mapper)
        {
            _db = uow;
            _mapper = mapper;
            _dividendPaymentRepo = uow.CreateRepository<DividendPayment>();
            _brokerAccountRepo = uow.CreateRepository<BrokerAccount>();
            _dividendRepo = uow.CreateRepository<Dividend>();
        }

        public async Task<IEnumerable<DividendPaymentDto>> GetAll(Guid? brokerAccountId, int pageIndex, int recordsQuantity)
        {
            var query = new ComplexQueryBuilder<DividendPayment>()
                .AddPagination(pageIndex, recordsQuantity,
                    dividendPayment => dividendPayment.ReceivedAt,
                    true)
                .AddJoins(DividendPaymentQuery.GetFullHierarchyColumns);

            if (brokerAccountId != null)
            {
                query.AddFilter(GetBaseFilter((Guid) brokerAccountId));
            }

            var dividends = await _dividendPaymentRepo
                .GetAll(query.GetQuery());
            
            return _mapper.Map<IEnumerable<DividendPaymentDto>>(dividends);
        }

        public async Task<PaginationConfigDto> GetPagination()
        {
            return await GetPaginationByFilter();
        }

        public async Task<PaginationConfigDto> GetPaginationByBrokerAccount(Guid brokerAccountId)
        {
            return await GetPaginationByFilter(GetBaseFilter(brokerAccountId));
        }

        private async Task<PaginationConfigDto> GetPaginationByFilter(Expression<Func<DividendPayment, bool>> filter = null)
        {
            int pageSize = 10;
            var recordsQuantity = await _dividendPaymentRepo.GetCount(filter);

            return new PaginationConfigDto()
            {
                PageSize = pageSize,
                RecordsQuantity = recordsQuantity
            };
        }

        private Expression<Func<DividendPayment, bool>> GetBaseFilter(Guid brokerAccountId)
        {
            return brokerAccountSecurity => brokerAccountSecurity.BrokerAccountId == brokerAccountId;
        }

        public async Task<decimal> GetEarnings()
        {
            return await _dividendPaymentRepo
                .GetSum(EarningAggregationExpression);
        }

        public async Task<decimal> GetEarningsByBrokerAccount(Guid brokerAccountId)
        {
            return await _dividendPaymentRepo
                .GetSum(EarningAggregationExpression, dividendPayment => dividendPayment.BrokerAccountId == brokerAccountId);
        }

        private static Expression<Func<DividendPayment, decimal>> EarningAggregationExpression =>
            dividendPayment => dividendPayment.SecuritiesQuantity * dividendPayment.Dividend.Amount - dividendPayment.Tax;

        public async Task<Guid> Add(DividendPaymentDto dividendPaymentDto)
        {
            var dividendPayment = _mapper.Map<DividendPayment>(dividendPaymentDto);
            dividendPayment.Id = Guid.NewGuid();
            await _dividendPaymentRepo.Add(dividendPayment);

            var dividend = await _dividendRepo.GetById(dividendPaymentDto.DividendId);
            await ActualizeBrokerAccountBalance(dividendPayment.BrokerAccountId,
                CalculateDividendPaymentAmount(dividend, dividendPayment.SecuritiesQuantity,
                    dividendPayment.Tax));
            await _db.Commit();
            return dividendPayment.Id;
        }

        public async Task Update(DividendPaymentDto dividendPaymentDto)
        {
            var dividendPayment = _mapper.Map<DividendPayment>(dividendPaymentDto);

            var existingDividend = await _dividendPaymentRepo.GetById(dividendPaymentDto.Id, DividendPaymentQuery.GetFullHierarchyColumns);
            var existingDividendAmount = CalculateDividendPaymentAmount(existingDividend.Dividend, existingDividend.SecuritiesQuantity,
                existingDividend.Tax);

            var actualDividend = dividendPaymentDto.DividendId != existingDividend.DividendId ? 
                await _dividendRepo.GetById(dividendPaymentDto.DividendId) : 
                existingDividend.Dividend;
            var actualDividendAmount = CalculateDividendPaymentAmount(actualDividend, dividendPaymentDto.SecuritiesQuantity,
                dividendPaymentDto.Tax);

            if (existingDividendAmount != actualDividendAmount)
            {
                await ActualizeBrokerAccountBalance(dividendPaymentDto.BrokerAccountId,
                    actualDividendAmount - existingDividendAmount);
            }

            _dividendPaymentRepo.Update(dividendPayment);
            await _db.Commit();
        }

        public async Task Delete(Guid id)
        {
            await _dividendPaymentRepo.Delete(id);

            var dividendPayment = await _dividendPaymentRepo.GetById(id, DividendPaymentQuery.GetFullHierarchyColumns);
            var diff = CalculateDividendPaymentAmount(dividendPayment.Dividend, dividendPayment.SecuritiesQuantity, 
                dividendPayment.Tax);
            await ActualizeBrokerAccountBalance(dividendPayment.BrokerAccountId, -1 * diff);

            await _db.Commit();
        }

        public async Task ActualizeBrokerAccountBalance(Guid brokerAccountId, decimal diff)
        {
            var brokerAccount = await _brokerAccountRepo.GetById(brokerAccountId, disableTracking: false);
            brokerAccount.MainCurrencyAmount += diff;
            _brokerAccountRepo.Update(brokerAccount);
        }

        private decimal CalculateDividendPaymentAmount(Dividend dividend, int securitiesQuantity, decimal tax)
        {
            return dividend.Amount * securitiesQuantity - tax;
        }
    }
}