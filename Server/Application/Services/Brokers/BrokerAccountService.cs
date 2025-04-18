﻿using System;
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

        public async Task<IEnumerable<BrokerAccountDTO>> GetAll()
        {
            var brokerAccounts = await _brokerAccountRepo.GetAll();
            return _mapper.Map<IEnumerable<BrokerAccountDTO>>(brokerAccounts);
        }

        public async Task<Guid> Add(BrokerAccountDTO brokerAccountDto)
        {
            var brokerAccount = _mapper.Map<BrokerAccount>(brokerAccountDto);
            brokerAccount.Id = Guid.NewGuid();
            brokerAccount.LastUpdateAt = DateTime.UtcNow;
            brokerAccount.AssetsValue = 0;
            await _brokerAccountRepo.Add(brokerAccount);
            _db.Commit();
            return brokerAccount.Id;
        }

        public async Task Update(BrokerAccountDTO brokerAccountDto)
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
