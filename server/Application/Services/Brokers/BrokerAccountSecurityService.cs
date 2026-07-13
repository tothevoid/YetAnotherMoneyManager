using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MoneyManager.Application.DTO.Brokers;
using MoneyManager.Application.DTO.Securities;
using MoneyManager.Application.Interfaces.Brokers;
using MoneyManager.Application.Interfaces.Integrations.Stock;
using MoneyManager.Application.Interfaces.Securities;
using MoneyManager.Application.Services.Securities;
using MoneyManager.Infrastructure.Entities.Brokers;
using MoneyManager.Infrastructure.Entities.Securities;
using MoneyManager.Infrastructure.Interfaces.Database;
using MoneyManager.Infrastructure.Interfaces.Messages;
using MoneyManager.Infrastructure.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text.Json;
using System.Threading.Tasks;

namespace MoneyManager.Application.Services.Brokers
{
    public class BrokerAccountSecurityService : IBrokerAccountSecurityService
    {
        private readonly IUnitOfWork _db;
        private readonly IMapper _mapper;

        private readonly IRepository<BrokerAccountSecurity> _brokerAccountSecurityRepo;
        private readonly IRepository<Security> _securityRepo;
        private readonly ISecurityService _securityService;
        private readonly IStockConnector _stockConnector;
        private readonly IPullQuotationsService _pullQuotationsService;

        private static readonly Expression<Func<BrokerAccountSecurity, object>> DefaultOrder = 
            (BrokerAccountSecurity brokerAccountSecurity) => brokerAccountSecurity.Security.Ticker;

        private IServerNotifier _serverNotifier;

        public BrokerAccountSecurityService(IUnitOfWork uow, IMapper mapper, 
            IStockConnector stockConnector, IServerNotifier serverNotifier,
            ISecurityService securityService,
            IPullQuotationsService pullQuotationsService)
        {
            _db = uow;
            _mapper = mapper;
            _brokerAccountSecurityRepo = uow.CreateRepository<BrokerAccountSecurity>();
            _securityRepo = uow.CreateRepository<Security>();
            _securityService = securityService;
            _serverNotifier = serverNotifier;
            _pullQuotationsService = pullQuotationsService;

            _stockConnector = stockConnector;
        }

        public async Task<IEnumerable<BrokerAccountSecurityDTO>> GetAll(bool unionSecurities = false)
        {
            var complexQuery = new ComplexQueryBuilder<BrokerAccountSecurity>()
                .AddJoins(GetFullHierarchyColumns)
                .AddOrder(DefaultOrder)
                .GetQuery();

            var brokerAccountSecurities = await _brokerAccountSecurityRepo
                .GetAll(complexQuery);

            if (!unionSecurities)
            {
                return _mapper.Map<IEnumerable<BrokerAccountSecurityDTO>>(brokerAccountSecurities);
            }

            var handledBrokerAccountSecurities = new Dictionary<Guid, BrokerAccountSecurity>();

            foreach (var brokerAccountSecurity in brokerAccountSecurities)
            {
                var securityId = brokerAccountSecurity.SecurityId;

                if (handledBrokerAccountSecurities.ContainsKey(securityId))
                {
                    var existingBrokerAccountSecurity = handledBrokerAccountSecurities[securityId];
                    existingBrokerAccountSecurity.Quantity += brokerAccountSecurity.Quantity;
                    existingBrokerAccountSecurity.Price += brokerAccountSecurity.Price;
                    existingBrokerAccountSecurity.SoldPrice += brokerAccountSecurity.SoldPrice;
                    existingBrokerAccountSecurity.SoldQuantity += brokerAccountSecurity.SoldQuantity;
                }
                else
                {
                    handledBrokerAccountSecurities.Add(securityId, brokerAccountSecurity);
                }
            }

            return _mapper.Map<IEnumerable<BrokerAccountSecurityDTO>>(handledBrokerAccountSecurities.Values);
        }

        public async Task<IEnumerable<BrokerAccountSecurityDTO>> GetByBrokerAccount(Guid brokerAccountId)
        {
            var complexQuery = new ComplexQueryBuilder<BrokerAccountSecurity>()
                .AddFilter(GetBaseFilter(brokerAccountId))
                .AddJoins(GetFullHierarchyColumns)
                .AddOrder(DefaultOrder)
                .GetQuery();

            var brokerAccountSecurities = await _brokerAccountSecurityRepo
                .GetAll(complexQuery);
            return _mapper.Map<IEnumerable<BrokerAccountSecurityDTO>>(brokerAccountSecurities);
        }

        public async Task PullQuotations()
        {
            var securities = (await _securityService.GetAll()).ToList();

            if (!securities.Any())
            {
                return;
            }

            await PullQuotations(securities);
        }

        public async Task PullQuotationsByBrokerAccount(Guid brokerAccountId)
        {
            //TODO: limit data to only ticker
            var brokerAccountSecurities = await _brokerAccountSecurityRepo
                .GetAll((brokerAccountSecurity) => brokerAccountSecurity.BrokerAccountId == brokerAccountId,
                    (query) => query.Include((brokerAccount) => brokerAccount.Security));

            var mappedSecurities =  _mapper.Map<IEnumerable<BrokerAccountSecurityDTO>>(brokerAccountSecurities);

            await PullQuotations(mappedSecurities.Select(brokerAccountSecurity => brokerAccountSecurity.Security).ToList());
        }

        private async Task PullQuotations(IEnumerable<SecurityDTO> securities)
        {
            var date = DateTime.UtcNow;
            var tickersValues = (await _stockConnector
                .GetValuesByTickers(securities)).ToList();
                
            var filteredValue = tickersValues
                .Where(marketValue => (marketValue.LastValue ?? marketValue.MarketPrice) != null)
                .OrderByDescending(marketValue => marketValue.Date)
                .DistinctBy(marketValue => marketValue.Ticker)
                .ToDictionary((marketValue) => marketValue.Ticker, (marketValue) => marketValue);
           
            foreach (var security in securities)
            {
                var row = filteredValue.GetValueOrDefault(security.Ticker);
                if (row == null) continue;

                // TODO: use service instead of repo
                var updatingSecurity = await _securityRepo.GetById(security.Id, null, false);
                updatingSecurity.ActualPrice = row.GetLastValue();
                updatingSecurity.PriceFetchedAt = DateTime.UtcNow;
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
            return securities.Sum(accountSecurity => (accountSecurity.Quantity - accountSecurity.SoldQuantity) * accountSecurity.Security.ActualPrice);
        }

        public async Task<decimal> GetTotalSoldByBrokerAccount(Guid brokerAccountId)
        {
            var brokerAccountSecurities = await GetByBrokerAccount(brokerAccountId);

            return brokerAccountSecurities.Sum(brokerAccountSecurity => brokerAccountSecurity.SoldPrice);
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
