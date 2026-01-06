using MoneyManager.Application.Interfaces.Brokers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using MoneyManager.Application.DTO.Brokers;
using MoneyManager.Infrastructure.Entities.Brokers;
using MoneyManager.Infrastructure.Interfaces.Database;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace MoneyManager.Application.Services.Brokers
{
    public class BrokerAccountTaxDeductionService : IBrokerAccountTaxDeductionService
    {
        private readonly IRepository<BrokerAccountTaxDeduction> _brokerAccountTaxDeductionRepo;
        private readonly IUnitOfWork _db;
        private readonly IMapper _mapper;

        public BrokerAccountTaxDeductionService(IUnitOfWork uow, IMapper mapper)
        {
            _db = uow;
            _mapper = mapper;
            _brokerAccountTaxDeductionRepo = uow.CreateRepository<BrokerAccountTaxDeduction>();
        }

        public async Task<IEnumerable<BrokerAccountTaxDeductionDto>> GetAll()
        {
            var entities = await _brokerAccountTaxDeductionRepo.GetAll(include: GetFullHierarchyColumns);
            return _mapper.Map<IEnumerable<BrokerAccountTaxDeductionDto>>(entities);
        }

        public async Task<decimal> GetAmountByBrokerAccount(Guid brokerAccountId)
        {
            return await _brokerAccountTaxDeductionRepo.GetSum(
                projection: (taxDeduction) => taxDeduction.Amount,
                filter: (taxDeduction) => taxDeduction.BrokerAccountId == brokerAccountId);
        }

        public async Task<Guid> Add(BrokerAccountTaxDeductionDto dto)
        {
            var entity = _mapper.Map<BrokerAccountTaxDeduction>(dto);
            entity.Id = Guid.NewGuid();
            await _brokerAccountTaxDeductionRepo.Add(entity);
            await _db.Commit();
            return entity.Id;
        }

        public async Task Update(BrokerAccountTaxDeductionDto dto)
        {
            var entity = _mapper.Map<BrokerAccountTaxDeduction>(dto);
            _brokerAccountTaxDeductionRepo.Update(entity);
            await _db.Commit();
        }

        public async Task Delete(Guid id)
        {
            await _brokerAccountTaxDeductionRepo.Delete(id);
            await _db.Commit();
        }

        private IQueryable<BrokerAccountTaxDeduction> GetFullHierarchyColumns(IQueryable<BrokerAccountTaxDeduction> taxDeductionQuery)
        {
            return taxDeductionQuery
                .Include(taxDeduction => taxDeduction.BrokerAccount.Type)
                .Include(taxDeduction => taxDeduction.BrokerAccount.Currency)
                .Include(taxDeduction => taxDeduction.BrokerAccount.Broker)
                .Include(taxDeduction => taxDeduction.BrokerAccount.Bank); ;
        }
    }
}