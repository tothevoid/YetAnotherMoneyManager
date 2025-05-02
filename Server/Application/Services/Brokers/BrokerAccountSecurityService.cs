using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MoneyManager.Application.DTO.Brokers;
using MoneyManager.Application.Interfaces.Brokers;
using MoneyManager.Infrastructure.Entities.Brokers;
using MoneyManager.Infrastructure.Interfaces.Database;

namespace MoneyManager.Application.Services.Brokers
{
    public class BrokerAccountSecurityService : IBrokerAccountSecurityService
    {
        private readonly IUnitOfWork _db;
        private readonly IMapper _mapper;

        private readonly IRepository<BrokerAccountSecurity> _brokerAccountSecurityRepo;
       
        public BrokerAccountSecurityService(IUnitOfWork uow, IMapper mapper)
        {
            _db = uow;
            _mapper = mapper;
            _brokerAccountSecurityRepo = uow.CreateRepository<BrokerAccountSecurity>();
        }

        public async Task<IEnumerable<BrokerAccountSecurityDTO>> GetAll(Guid brokerAccountId, 
            int recordsQuantity, int pageIndex)
        {
            var brokerAccountSecurities = await _brokerAccountSecurityRepo
                .GetAll(GetBaseFilter(brokerAccountId), GetFullHierarchyColumns,
                    recordsQuantity,
                    (pageIndex - 1) * recordsQuantity);
            return _mapper.Map<IEnumerable<BrokerAccountSecurityDTO>>(brokerAccountSecurities);
        }

        public async Task<IEnumerable<BrokerAccountSecurityDTO>> GetByBrokerAccount(Guid brokerAccountId)
        {
            var brokerAccountSecurities = await _brokerAccountSecurityRepo
                .GetAll(GetBaseFilter(brokerAccountId), GetFullHierarchyColumns);
            return _mapper.Map<IEnumerable<BrokerAccountSecurityDTO>>(brokerAccountSecurities);
        }

        public async Task<BrokerAccountSecurityPaginationDto> GetPagination(Guid brokerAccountId)
        {
            int pageSize = 20;
            var recordsQuantity = await _brokerAccountSecurityRepo.GetCount(GetBaseFilter(brokerAccountId));

            return new BrokerAccountSecurityPaginationDto()
            {
                PageSize = pageSize,
                PagesQuantity = (int) Math.Ceiling(recordsQuantity * 1.0 / pageSize)
            };
        }

        public async Task<Guid> Add(BrokerAccountSecurityDTO brokerAccountSecurityDto)
        {
            var brokerAccountSecurity = _mapper.Map<BrokerAccountSecurity>(brokerAccountSecurityDto);
            brokerAccountSecurity.Id = Guid.NewGuid();
            await _brokerAccountSecurityRepo.Add(brokerAccountSecurity);
            await _db.Commit();
            return brokerAccountSecurity.Id;
        }

        public async Task Update(BrokerAccountSecurityDTO brokerAccountSecurityDto)
        {
            var brokerAccountSecurity = _mapper.Map<BrokerAccountSecurity>(brokerAccountSecurityDto);
            _brokerAccountSecurityRepo.Update(brokerAccountSecurity);
            await _db.Commit();
        }

        public async Task Delete(Guid id)
        {
            await _brokerAccountSecurityRepo.Delete(id);
            await _db.Commit();
        }

        private Expression<Func<BrokerAccountSecurity, bool>> GetBaseFilter(Guid brokerAccountId)
        {
            return brokerAccountSecurity => brokerAccountSecurity.BrokerAccountId == brokerAccountId;
        }

        private IQueryable<BrokerAccountSecurity> GetFullHierarchyColumns(IQueryable<BrokerAccountSecurity> brokerAccountSecurityQuery)
        {
            return brokerAccountSecurityQuery
                .Include(brokerAccountSecurity => brokerAccountSecurity.Security.Type)
                .Include(brokerAccountSecurity => brokerAccountSecurity.BrokerAccount.Type)
                .Include(brokerAccountSecurity => brokerAccountSecurity.BrokerAccount.Currency)
                .Include(brokerAccountSecurity => brokerAccountSecurity.BrokerAccount.Broker);
        }
    }
}
