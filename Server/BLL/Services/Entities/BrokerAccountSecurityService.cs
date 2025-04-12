using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using BLL.DTO;
using MoneyManager.BLL.Interfaces.Entities;
using MoneyManager.DAL.Entities;
using MoneyManager.DAL.Interfaces;

namespace MoneyManager.BLL.Services.Entities
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

        public async Task<IEnumerable<BrokerAccountSecurityDto>> GetAll()
        {
            var brokerAccountSecurities = await _brokerAccountSecurityRepo.GetAll();
            return _mapper.Map<IEnumerable<BrokerAccountSecurityDto>>(brokerAccountSecurities);
        }

        public async Task<Guid> Add(BrokerAccountSecurityDto brokerAccountSecurityDto)
        {
            var brokerAccountSecurity = _mapper.Map<BrokerAccountSecurity>(brokerAccountSecurityDto);
            brokerAccountSecurity.Id = Guid.NewGuid();
            await _brokerAccountSecurityRepo.Add(brokerAccountSecurity);
            _db.Commit();
            return brokerAccountSecurity.Id;
        }

        public async Task Update(BrokerAccountSecurityDto brokerAccountSecurityDto)
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
