using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MoneyManager.Application.DTO.Brokers;
using MoneyManager.Application.Interfaces.Brokers;
using MoneyManager.Application.Interfaces.Integrations.Stock;
using MoneyManager.Infrastructure.Entities.Brokers;
using MoneyManager.Infrastructure.Interfaces.Database;
using MoneyManager.Infrastructure.Queries;

namespace MoneyManager.Application.Services.Brokers
{
    public class BrokerAccountService : IBrokerAccountService
    {
        private readonly IUnitOfWork _db;

        private readonly IRepository<BrokerAccount> _brokerAccountRepo;
        private readonly IBrokerAccountSecurityService _brokerAccountService;

        private readonly IMapper _mapper;
       
        public BrokerAccountService(IUnitOfWork uow, IMapper mapper, 
            IBrokerAccountSecurityService brokerAccountSecurityService)
        {
            _db = uow;
            _mapper = mapper;
            _brokerAccountService = brokerAccountSecurityService;
            _brokerAccountRepo = uow.CreateRepository<BrokerAccount>();
        }

        public async Task<IEnumerable<BrokerAccountDTO>> GetAll()
        {
            var query = new ComplexQueryBuilder<BrokerAccount>()
                .AddOrder(brokerAccount => brokerAccount.Name)
                .AddJoins(GetFullHierarchyColumns)
                .GetQuery();

            var brokerAccounts = await _brokerAccountRepo
                .GetAll(query);

            var brokerAccountsDtos = _mapper.Map<IEnumerable<BrokerAccountDTO>>(brokerAccounts)
                .ToList();

            // TODO: make it on DB level
            foreach (var account in brokerAccountsDtos)
            {
                await ApplyAssetsValue(account);
            }

            return brokerAccountsDtos;
        }


        public async Task<BrokerAccountDTO> GetById(Guid id)
        {
            var brokerAccount = await _brokerAccountRepo.GetById(id, GetFullHierarchyColumns);
            var brokerAccountDto = _mapper.Map<BrokerAccountDTO>(brokerAccount);
            await ApplyAssetsValue(brokerAccountDto);
            return brokerAccountDto;
        }

        public async Task<Guid> Add(BrokerAccountDTO brokerAccountDto)
        {
            var brokerAccount = _mapper.Map<BrokerAccount>(brokerAccountDto);
            brokerAccount.Id = Guid.NewGuid();
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

        private async Task ApplyAssetsValue(BrokerAccountDTO brokerAccount)
        {
            var securities = await _brokerAccountService
                .GetByBrokerAccount(brokerAccount.Id);

            var mainCurrencyAmount = brokerAccount.MainCurrencyAmount * brokerAccount.Currency.Rate;

            brokerAccount.CurrentValue = securities.Sum(accountSecurity => 
                accountSecurity.Quantity * accountSecurity.Security.ActualPrice) + mainCurrencyAmount;
            brokerAccount.InitialValue = securities.Sum(accountSecurity =>
                accountSecurity.Price) + mainCurrencyAmount;
        }

        private IQueryable<BrokerAccount> GetFullHierarchyColumns(IQueryable<BrokerAccount> brokerAccountQuery)
        {
            return brokerAccountQuery
                .Include(brokerAccount => brokerAccount.Type)
                .Include(brokerAccount => brokerAccount.Currency)
                .Include(brokerAccount => brokerAccount.Broker);
        }

        public Task<BrokerAccountSummaryDto> GetSummary(Guid brokerAccountId)
        {
            throw new NotImplementedException();
        }
    }
}
