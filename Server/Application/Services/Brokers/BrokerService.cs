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

        public async Task<IEnumerable<BrokerDTO>> GetAll()
        {
            var brokers = await _brokerRepo.GetAll();
            return _mapper.Map<IEnumerable<BrokerDTO>>(brokers);
        }

         public async Task<Guid> Add(BrokerDTO securityDto)
        {
            var broker = _mapper.Map<Broker>(securityDto);
            broker.Id = Guid.NewGuid();
            await _brokerRepo.Add(broker);
            _db.Commit();
            return broker.Id;
        }

        public async Task Update(BrokerDTO brokersDto)
        {
            var brokers = _mapper.Map<Broker>(brokersDto);
            await _brokerRepo.Update(brokers);
            _db.Commit();
        }

        public async Task Delete(Guid id)
        {
            await _brokerRepo.Delete(id);
            _db.Commit();
        }
    }
}
