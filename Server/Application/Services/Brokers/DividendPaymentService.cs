using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MoneyManager.Application.DTO.Brokers;
using MoneyManager.Application.Interfaces.Brokers;
using MoneyManager.Infrastructure.Entities.Brokers;
using MoneyManager.Infrastructure.Entities.Securities;
using MoneyManager.Infrastructure.Interfaces.Database;

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

        public async Task<IEnumerable<DividendPaymentDto>> GetAll(Guid brokerAccountId)
        {
            var dividends = await _dividendPaymentRepo
                .GetAll(dividendPayment => dividendPayment.BrokerAccountId == brokerAccountId, 
                    GetFullHierarchyColumns);
            
            return _mapper.Map<IEnumerable<DividendPaymentDto>>(dividends);
        }

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

            var existingDividend = await _dividendPaymentRepo.GetById(dividendPaymentDto.Id, GetFullHierarchyColumns);
            var existingDividendAmount = CalculateDividendPaymentAmount(dividendPayment.Dividend, dividendPayment.SecuritiesQuantity,
                dividendPayment.Tax);

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

            var dividendPayment = await _dividendPaymentRepo.GetById(id, GetFullHierarchyColumns);
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

        private IQueryable<DividendPayment> GetFullHierarchyColumns(IQueryable<DividendPayment> dividendPaymentQuery)
        {
            return dividendPaymentQuery
                .Include(dividendPayment => dividendPayment.Dividend.Security.Currency)
                .Include(dividendPayment => dividendPayment.Dividend.Security.Type)
                .Include(dividendPayment => dividendPayment.BrokerAccount.Type)
                .Include(dividendPayment => dividendPayment.BrokerAccount.Currency)
                .Include(dividendPayment => dividendPayment.BrokerAccount.Broker);

        }
    }
}