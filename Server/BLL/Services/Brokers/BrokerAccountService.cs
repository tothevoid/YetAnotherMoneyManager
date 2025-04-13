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
    public class BrokerAccountService : IBrokerAccountService
    {
        private readonly IUnitOfWork _db;

        private readonly IRepository<BrokerAccount> _brokerAccountRepo;
        private readonly IMapper _mapper;
        public BrokerAccountService(IUnitOfWork uow, IMapper mapper)
        {
            _db = uow;
            _mapper = mapper;
            _brokerAccountRepo = uow.CreateRepository<BrokerAccount>();
        }

        public async Task<IEnumerable<BrokerAccountDto>> GetAll()
        {
            var brokerAccounts = await _brokerAccountRepo.GetAll();
            return _mapper.Map<IEnumerable<BrokerAccountDto>>(brokerAccounts);
        }

        public async Task<Guid> Add(BrokerAccountDto brokerAccountDto)
        {
            var brokerAccount = _mapper.Map<BrokerAccount>(brokerAccountDto);
            brokerAccount.Id = Guid.NewGuid();
            await _brokerAccountRepo.Add(brokerAccount);
            _db.Commit();
            return brokerAccount.Id;
        }

        public async Task Update(BrokerAccountDto brokerAccountDto)
        {
            var brokerAccount = _mapper.Map<BrokerAccount>(brokerAccountDto);
            await _brokerAccountRepo.Update(brokerAccount);
            _db.Commit();
        }

        public async Task Delete(Guid id)
        {
            await _brokerAccountRepo.Delete(id);
            _db.Commit();
        }
    }
}
