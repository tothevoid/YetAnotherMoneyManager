using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using MoneyManager.Application.DTO.Brokers;
using MoneyManager.Application.Interfaces.Brokers;
using MoneyManager.Infrastructure.Entities.Brokers;
using MoneyManager.Infrastructure.Interfaces.Database;

namespace MoneyManager.Application.Services.Brokers
{
    public class BrokerAccountSecurityService : IBrokerAccountSecurityService
    {
        private readonly IUnitOfWork _db;

        private readonly IRepository<BrokerAccountSecurity> _brokerAccountSecurityRepo;
        private readonly IMapper _mapper;
        public BrokerAccountSecurityService(IUnitOfWork uow, IMapper mapper)
        {
            _db = uow;
            _mapper = mapper;
            _brokerAccountSecurityRepo = uow.CreateRepository<BrokerAccountSecurity>();
        }

        public async Task<IEnumerable<BrokerAccountSecurityDTO>> GetAll()
        {
            var brokerAccountSecurities = await _brokerAccountSecurityRepo.GetAll();
            return _mapper.Map<IEnumerable<BrokerAccountSecurityDTO>>(brokerAccountSecurities);
        }

        public async Task<IEnumerable<BrokerAccountSecurityDTO>> GetByBrokerAccount(Guid brokerAccountId)
        {
            var brokerAccountSecurities = await _brokerAccountSecurityRepo
                .GetAll(brokerAccountSecurity => brokerAccountSecurity.BrokerAccountId == brokerAccountId);
            return _mapper.Map<IEnumerable<BrokerAccountSecurityDTO>>(brokerAccountSecurities);
        }

        public async Task<Guid> Add(BrokerAccountSecurityDTO brokerAccountSecurityDto)
        {
            var brokerAccountSecurity = _mapper.Map<BrokerAccountSecurity>(brokerAccountSecurityDto);
            brokerAccountSecurity.Id = Guid.NewGuid();
            await _brokerAccountSecurityRepo.Add(brokerAccountSecurity);
            _db.Commit();
            return brokerAccountSecurity.Id;
        }

        public async Task Update(BrokerAccountSecurityDTO brokerAccountSecurityDto)
        {
            var brokerAccountSecurity = _mapper.Map<BrokerAccountSecurity>(brokerAccountSecurityDto);
            await _brokerAccountSecurityRepo.Update(brokerAccountSecurity);
            _db.Commit();
        }

        public async Task Delete(Guid id)
        {
            await _brokerAccountSecurityRepo.Delete(id);
            _db.Commit();
        }
    }
}
