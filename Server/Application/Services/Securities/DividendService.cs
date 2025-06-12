using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MoneyManager.Application.DTO.Brokers;
using MoneyManager.Application.DTO.Securities;
using MoneyManager.Application.Interfaces.Securities;
using MoneyManager.Infrastructure.Entities.Deposits;
using MoneyManager.Infrastructure.Entities.Securities;
using MoneyManager.Infrastructure.Interfaces.Database;

namespace MoneyManager.Application.Services.Securities
{
    public class DividendService : IDividendService
    {
        private readonly IUnitOfWork _db;

        private readonly IRepository<Dividend> _dividendRepo;
        private readonly IMapper _mapper;
        public DividendService(IUnitOfWork uow, IMapper mapper)
        {
            _db = uow;
            _mapper = mapper;
            _dividendRepo = uow.CreateRepository<Dividend>();
        }

        public async Task<IEnumerable<DividendDto>> GetAll(Guid securityId)
        {
            var securities = await _dividendRepo
                .GetAll((dividend) => dividend.SecurityId == securityId,
                    include: GetFullHierarchyColumns);
            return _mapper.Map<IEnumerable<DividendDto>>(securities);
        }

        public async Task<IEnumerable<DividendDto>> GetAvailable(Guid brokerAccountId)
        {
            var securities = await _dividendRepo
                .GetAll((dividend) => dividend.DividendPayments.All(payment => 
                        payment.BrokerAccountId == brokerAccountId && 
                        payment.BrokerAccount.BrokerAccountSecurities.Any(security => security.Id == dividend.SecurityId)),
                    include: GetFullHierarchyColumns);
            return _mapper.Map<IEnumerable<DividendDto>>(securities);
        }

        public async Task Update(DividendDto securityTypeDto)
        {
            var dividend = _mapper.Map<Dividend>(securityTypeDto);
            _dividendRepo.Update(dividend);
            await _db.Commit();
        }

        public async Task<Guid> Add(DividendDto securityDto)
        {
            var dividend = _mapper.Map<Dividend>(securityDto);
            dividend.Id = Guid.NewGuid();
            await _dividendRepo.Add(dividend);
            await _db.Commit();
            return dividend.Id;
        }

        public async Task Delete(Guid id)
        {
            await _dividendRepo.Delete(id);
            await _db.Commit();
        }

        private IQueryable<Dividend> GetFullHierarchyColumns(IQueryable<Dividend> dividendQuery)
        {
            return dividendQuery.Include(dividend => dividend.Security.Currency);
        }
    }
}
