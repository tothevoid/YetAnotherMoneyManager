using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MoneyManager.Application.DTO.Brokers;
using MoneyManager.Application.Interfaces.Brokers;
using MoneyManager.Application.Mappings;
using MoneyManager.Infrastructure.Entities.Brokers;
using MoneyManager.Infrastructure.Interfaces.Database;

namespace MoneyManager.Application.Services.Brokers
{
    public class BrokerService : IBrokerService
    {
        private readonly IUnitOfWork _db;

        private readonly IRepository<Broker> _brokerRepo;
        private readonly ApplicationMapper _mapper;
        public BrokerService(IUnitOfWork uow, ApplicationMapper mapper)
        {
            _db = uow;
            _mapper = mapper;
            _brokerRepo = uow.CreateRepository<Broker>();
        }

        public async Task<IEnumerable<BrokerDto>> GetAllAsync()
        {
            var brokers = await _brokerRepo.GetAllAsync();
            return _mapper.Map(brokers);
        }

        public async Task<Guid> AddAsync(BrokerDto securityDto)
        {
            var broker = _mapper.Map(securityDto);
            broker.Id = Guid.NewGuid();
            await _brokerRepo.AddAsync(broker);
            await _db.CommitAsync();
            return broker.Id;
        }

        public async Task UpdateAsync(BrokerDto brokersDto)
        {
            var brokers = _mapper.Map(brokersDto);
            _brokerRepo.Update(brokers);
            await _db.CommitAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            await _brokerRepo.DeleteAsync(id);
            await _db.CommitAsync();
        }
    }
}
