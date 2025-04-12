using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using MoneyManager.BLL.DTO;
using MoneyManager.BLL.Interfaces.Entities;
using MoneyManager.DAL.Entities;
using MoneyManager.DAL.Interfaces;

namespace MoneyManager.BLL.Services.Entities
{
    public class BrokerService : IBrokerService
    {
        private readonly IUnitOfWork _db;

        private readonly IRepository<Broker> _brokerRepo;
        private readonly IMapper _mapper;
        public BrokerService(IUnitOfWork uow, IMapper mapper)
        {
            _db = uow;
            _mapper = mapper;
            _brokerRepo = uow.CreateRepository<Broker>();
        }

        public async Task<IEnumerable<BrokerDto>> GetAll()
        {
            var brokers = await _brokerRepo.GetAll();
            return _mapper.Map<IEnumerable<BrokerDto>>(brokers);
        }

        public async Task Update(BrokerDto brokersDto)
        {
            var brokers = _mapper.Map<Broker>(brokersDto);
            await _brokerRepo.Update(brokers);
            _db.Commit();
        }

        public async Task<Guid> Add(BrokerDto securityDto)
        {
            var broker = _mapper.Map<Broker>(securityDto);
            broker.Id = Guid.NewGuid();
            await _brokerRepo.Add(broker);
            _db.Commit();
            return broker.Id;
        }

        public async Task Delete(Guid id)
        {
            await _brokerRepo.Delete(id);
            _db.Commit();
        }
    }
}
