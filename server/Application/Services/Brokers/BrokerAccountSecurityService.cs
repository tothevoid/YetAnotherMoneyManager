using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MoneyManager.Application.DTO.Brokers;
using MoneyManager.Application.Interfaces.Brokers;
using MoneyManager.Application.Interfaces.Integrations.Stock;
using MoneyManager.Application.Interfaces.Securities;
using MoneyManager.Application.Services.Securities;
using MoneyManager.Infrastructure.Entities.Brokers;
using MoneyManager.Infrastructure.Entities.Securities;
using MoneyManager.Infrastructure.Interfaces.Database;
using MoneyManager.Infrastructure.Interfaces.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Text.Json;

namespace MoneyManager.Application.Services.Brokers
{
    public class BrokerAccountSecurityService : IBrokerAccountSecurityService
    {
        private readonly IUnitOfWork _db;
        private readonly IMapper _mapper;

        private readonly IRepository<BrokerAccountSecurity> _brokerAccountSecurityRepo;
        private readonly IRepository<Security> _securityRepo;
        private readonly IStockConnector _stockConnector;
        private readonly IPullQuotationsService _pullQuotationsService;

        private IServerNotifier _serverNotifier;

        public BrokerAccountSecurityService(IUnitOfWork uow, IMapper mapper, 
            IStockConnector stockConnector, IServerNotifier serverNotifier,
            IPullQuotationsService pullQuotationsService)
        {
            _db = uow;
            _mapper = mapper;
            _brokerAccountSecurityRepo = uow.CreateRepository<BrokerAccountSecurity>();
            _securityRepo = uow.CreateRepository<Security>();
            _serverNotifier = serverNotifier;
            _pullQuotationsService = pullQuotationsService;

            _stockConnector = stockConnector;
        }

        public async Task<IEnumerable<BrokerAccountSecurityDTO>> GetAll()
        {
            var brokerAccountSecurities = await _brokerAccountSecurityRepo
                .GetAll(include: GetFullHierarchyColumns);
            return _mapper.Map<IEnumerable<BrokerAccountSecurityDTO>>(brokerAccountSecurities);
        }

        public async Task<IEnumerable<BrokerAccountSecurityDTO>> GetByBrokerAccount(Guid brokerAccountId)
        {
            var brokerAccountSecurities = await _brokerAccountSecurityRepo
                .GetAll(GetBaseFilter(brokerAccountId), GetFullHierarchyColumns);
            return _mapper.Map<IEnumerable<BrokerAccountSecurityDTO>>(brokerAccountSecurities);
        }

        public async Task PullQuotations()
        {
            var securities = await _securityRepo.GetAll();
            var tickers = securities.Select(security => security.Ticker).ToList();

            if (!tickers.Any())
            {
                return;
            }

            await PullQuotations(tickers);
        }

        public async Task PullQuotations(Guid brokerAccountId)
        {
            //TODO: limit data to only ticker
            var brokerAccountSecurities = await _brokerAccountSecurityRepo
                .GetAll((brokerAccountSecurity) => brokerAccountSecurity.BrokerAccountId == brokerAccountId,
                    (query) => query.Include((brokerAccount) => brokerAccount.Security));

            var tickers = brokerAccountSecurities
                .Select(brokerAccountSecurity => brokerAccountSecurity.Security.Ticker)
                .ToArray();

            await PullQuotations(tickers);
        }

        private async Task PullQuotations(IEnumerable<string> tickers)
        {
            var date = DateTime.UtcNow;
            var tickersValues = (await _stockConnector
                .GetValuesByTickers(tickers)).ToList();
                
            var filteredValue = tickersValues
                .Where(marketValue => (marketValue.LastValue ?? marketValue.MarketPrice) != null)
                .OrderByDescending(marketValue => marketValue.Date)
                .DistinctBy(marketValue => marketValue.Ticker)
                .ToDictionary((marketValue) => marketValue.Ticker, (marketValue) => marketValue);

            //TODO: possible reuse brokerAccountSecurities
            var brokerAccountSecuritiesToUpdated = await _securityRepo
              .GetAll((security) => tickers.Contains(security.Ticker),
                  disableTracking: false);

            foreach (var security in brokerAccountSecuritiesToUpdated)
            {
                var row = filteredValue.GetValueOrDefault(security.Ticker);
                security.ActualPrice = row.LastValue ?? row.MarketPrice ?? 0;
                security.PriceFetchedAt = DateTime.UtcNow;
            }

            _pullQuotationsService.UpdatePullDate(date);
            await _db.Commit();
            await _serverNotifier.SendToAll(JsonSerializer.Serialize(new { date }));
        }

        public async Task<Guid> Add(BrokerAccountSecurityDTO brokerAccountSecurityDto)
        {
            var brokerAccountSecurity = _mapper.Map<BrokerAccountSecurity>(brokerAccountSecurityDto);
            brokerAccountSecurity.Id = Guid.NewGuid();
            await _brokerAccountSecurityRepo.Add(brokerAccountSecurity);
            await _db.Commit();
            return brokerAccountSecurity.Id;
        }

        public async Task Update(BrokerAccountSecurityDTO brokerAccountSecurityDto)
        {
            var brokerAccountSecurity = _mapper.Map<BrokerAccountSecurity>(brokerAccountSecurityDto);
            _brokerAccountSecurityRepo.Update(brokerAccountSecurity);
            await _db.Commit();
        }

        public async Task Delete(Guid id)
        {
            await _brokerAccountSecurityRepo.Delete(id);
            await _db.Commit();
        }

        public async Task<decimal> GetInitialSecuritiesValue(Guid brokerAccountId)
        {
            var securities = await GetByBrokerAccount(brokerAccountId);
            return securities.Sum(accountSecurity => accountSecurity.Price);
        }

        public async Task<decimal> GetActualSecuritiesValue(Guid brokerAccountId)
        {
            var securities = await GetByBrokerAccount(brokerAccountId);
            return securities.Sum(accountSecurity => accountSecurity.Quantity * accountSecurity.Security.ActualPrice);
        }

        private Expression<Func<BrokerAccountSecurity, bool>> GetBaseFilter(Guid brokerAccountId)
        {
            return brokerAccountSecurity => brokerAccountSecurity.BrokerAccountId == brokerAccountId;
        }

        private IQueryable<BrokerAccountSecurity> GetFullHierarchyColumns(IQueryable<BrokerAccountSecurity> brokerAccountSecurityQuery)
        {
            return brokerAccountSecurityQuery
                .Include(brokerAccountSecurity => brokerAccountSecurity.Security.Currency)
                .Include(brokerAccountSecurity => brokerAccountSecurity.Security.Type)
                .Include(brokerAccountSecurity => brokerAccountSecurity.BrokerAccount.Type)
                .Include(brokerAccountSecurity => brokerAccountSecurity.BrokerAccount.Currency)
                .Include(brokerAccountSecurity => brokerAccountSecurity.BrokerAccount.Broker)
                .Include(brokerAccountSecurity => brokerAccountSecurity.BrokerAccount.Bank);
        }
    }
}
