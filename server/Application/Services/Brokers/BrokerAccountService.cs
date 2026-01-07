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
        private readonly IBrokerAccountSecurityService _brokerAccountSecurityService;

        private readonly IMapper _mapper;
       
        public BrokerAccountService(IUnitOfWork uow, IMapper mapper, 
            IBrokerAccountSecurityService brokerAccountSecuritySecurityService)
        {
            _db = uow;
            _mapper = mapper;
            _brokerAccountSecurityService = brokerAccountSecuritySecurityService;
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
            foreach (var brokerAccount in brokerAccountsDtos)
            {
                brokerAccount.ApplyPortfolioValues(await GetPortfolioValues(brokerAccount));
            }

            return brokerAccountsDtos;
        }
        public async Task<BrokerAccountDTO> GetById(Guid id)
        {
            var brokerAccount = await _brokerAccountRepo.GetById(id, GetFullHierarchyColumns);
            var brokerAccountDto = _mapper.Map<BrokerAccountDTO>(brokerAccount);
            brokerAccountDto.ApplyPortfolioValues(await GetPortfolioValues(brokerAccountDto));
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

        public async Task<BrokerAccountPortfolioDto> GetPortfolioValuesByBrokerAccount(Guid brokerAccountId)
        {
            var brokerAccount = await GetById(brokerAccountId);

            return await GetPortfolioValues(brokerAccount);
        }

        public async Task<BrokerAccountPortfolioDto> GetPortfolioValues()
        {
            var brokerAccounts = await GetAll();

            var portfolioValues = new BrokerAccountPortfolioDto();

            // TODO: Possible different currencies
            foreach (var brokerAccount in brokerAccounts)
            {
                var portfolioValue = await GetPortfolioValues(brokerAccount);

                portfolioValues.InitialValue += portfolioValue.InitialValue;
                portfolioValues.CurrentValue += portfolioValue.CurrentValue;
            }

            return portfolioValues;
        }

        private async Task<BrokerAccountPortfolioDto> GetPortfolioValues(BrokerAccountDTO brokerAccount)
        {
            var mainCurrencyAmount = brokerAccount.MainCurrencyAmount * brokerAccount.Currency.Rate;

            // TODO: Use single query to get both values
            var currentSecuritiesValue = await _brokerAccountSecurityService.GetActualSecuritiesValue(brokerAccount.Id);
            var initialSecuritiesValue = await _brokerAccountSecurityService.GetInitialSecuritiesValue(brokerAccount.Id);

            brokerAccount.CurrentValue = currentSecuritiesValue + mainCurrencyAmount;
            brokerAccount.InitialValue = initialSecuritiesValue + mainCurrencyAmount;

            return new BrokerAccountPortfolioDto
            {
                CurrentValue = currentSecuritiesValue + mainCurrencyAmount,
                InitialValue = initialSecuritiesValue + mainCurrencyAmount
            };
        }

        private IQueryable<BrokerAccount> GetFullHierarchyColumns(IQueryable<BrokerAccount> brokerAccountQuery)
        {
            return brokerAccountQuery
                .Include(brokerAccount => brokerAccount.Type)
                .Include(brokerAccount => brokerAccount.Currency)
                .Include(brokerAccount => brokerAccount.Broker)
                .Include(brokerAccount => brokerAccount.Bank);
        }

        
    }
}
