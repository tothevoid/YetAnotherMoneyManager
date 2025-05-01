using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
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

        public async Task<IEnumerable<BrokerAccountDTO>> GetAll()
        {
            var brokerAccounts = await _brokerAccountRepo
                .GetAll(include: GetFullHierarchyColumns);
            return _mapper.Map<IEnumerable<BrokerAccountDTO>>(brokerAccounts);
        }

        public async Task<BrokerAccountDTO> GetById(Guid id)
        {
            var brokerAccount = await _brokerAccountRepo.GetById(id, GetFullHierarchyColumns);
            return _mapper.Map<BrokerAccountDTO>(brokerAccount);
        }

        public async Task<Guid> Add(BrokerAccountDTO brokerAccountDto)
        {
            var brokerAccount = _mapper.Map<BrokerAccount>(brokerAccountDto);
            brokerAccount.Id = Guid.NewGuid();
            brokerAccount.LastUpdateAt = DateTime.UtcNow;
            brokerAccount.AssetsValue = 0;
            await _brokerAccountRepo.Add(brokerAccount);
            await _db.Commit();
            return brokerAccount.Id;
        }

        public async Task Update(BrokerAccountDTO brokerAccountDto)
        {
            var brokerAccount = _mapper.Map<BrokerAccount>(brokerAccountDto);
            _brokerAccountRepo.Update(brokerAccount);
            await _db.Commit();
        }

        public async Task Delete(Guid id)
        {
            await _brokerAccountRepo.Delete(id);
            await _db.Commit();
        }

        private IQueryable<BrokerAccount> GetFullHierarchyColumns(IQueryable<BrokerAccount> brokerAccountQuery)
        {
            return brokerAccountQuery
                .Include(brokerAccount => brokerAccount.Type)
                .Include(brokerAccount => brokerAccount.Currency)
                .Include(brokerAccount => brokerAccount.Broker);
        }
    }
}
