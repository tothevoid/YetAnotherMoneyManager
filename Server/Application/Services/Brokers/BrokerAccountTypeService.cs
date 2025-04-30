using AutoMapper;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using MoneyManager.Application.DTO.Brokers;
using MoneyManager.Application.Interfaces.Brokers;
using MoneyManager.Infrastructure.Interfaces.Database;
using MoneyManager.Infrastructure.Entities.Brokers;

namespace MoneyManager.Application.Services.Brokers
{
    public class BrokerAccountTypeService : IBrokerAccountTypeService
    {
        private readonly IUnitOfWork _db;
        private readonly IRepository<BrokerAccountType> _brokerAccountTypeRepo;
        private readonly IMapper _mapper;
        public BrokerAccountTypeService(IUnitOfWork uow, IMapper mapper)
        {
            _db = uow;
            _mapper = mapper;
            _brokerAccountTypeRepo = uow.CreateRepository<BrokerAccountType>();
        }

        public async Task<IEnumerable<BrokerAccountTypeDTO>> GetAll()
        {
            var brokerAccountTypes = await _brokerAccountTypeRepo.GetAll();
            return _mapper.Map<IEnumerable<BrokerAccountTypeDTO>>(brokerAccountTypes);
        }

        public async Task<Guid> Add(BrokerAccountTypeDTO brokerAccountTypeDto)
        {
            var brokerAccountType = _mapper.Map<BrokerAccountType>(brokerAccountTypeDto);
            brokerAccountType.Id = Guid.NewGuid();
            await _brokerAccountTypeRepo.Add(brokerAccountType);
            await _db.Commit();
            return brokerAccountType.Id;
        }

        public async Task Update(BrokerAccountTypeDTO brokerAccountTypeDto)
        {
            var brokerAccountType = _mapper.Map<BrokerAccountType>(brokerAccountTypeDto);
            _brokerAccountTypeRepo.Update(brokerAccountType);
            await _db.Commit();
        }

        public async Task Delete(Guid id)
        {
            await _brokerAccountTypeRepo.Delete(id);
            await _db.Commit();
        }
    }
}
