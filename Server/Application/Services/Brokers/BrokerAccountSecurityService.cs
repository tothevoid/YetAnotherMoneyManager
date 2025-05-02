using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Principal;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MoneyManager.Application.DTO.Brokers;
using MoneyManager.Application.Interfaces.Brokers;
using MoneyManager.Application.Interfaces.Integrations.Stock;
using MoneyManager.Infrastructure.Entities.Brokers;
using MoneyManager.Infrastructure.Interfaces.Database;

namespace MoneyManager.Application.Services.Brokers
{
    public class BrokerAccountSecurityService : IBrokerAccountSecurityService
    {
        private readonly IUnitOfWork _db;
        private readonly IMapper _mapper;

        private readonly IRepository<BrokerAccountSecurity> _brokerAccountSecurityRepo;
        private readonly IStockConnector _stockConnector;

        public BrokerAccountSecurityService(IUnitOfWork uow, IMapper mapper, IStockConnector stockConnector)
        {
            _db = uow;
            _mapper = mapper;
            _brokerAccountSecurityRepo = uow.CreateRepository<BrokerAccountSecurity>();
            _stockConnector = stockConnector;
        }

        public async Task<IEnumerable<BrokerAccountSecurityDTO>> GetByBrokerAccount(Guid brokerAccountId)
        {
            var brokerAccountSecurities = await _brokerAccountSecurityRepo
                .GetAll(GetBaseFilter(brokerAccountId), GetFullHierarchyColumns);
            return _mapper.Map<IEnumerable<BrokerAccountSecurityDTO>>(brokerAccountSecurities);
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

            var tickersValues = await _stockConnector.GetValuesByTickers(tickers);

            //TODO: possible reuse brokerAccountSecurities
            var brokerAccountSecuritiesToUpdated = await _brokerAccountSecurityRepo
              .GetAll((brokerAccountSecurity) => tickers.Contains(brokerAccountSecurity.Security.Ticker),
              (query) => query.Include((brokerAccount) => brokerAccount.Security),
              disableTracking: false);

            foreach (var brokerAccountSecurity in brokerAccountSecuritiesToUpdated)
            {
                var value = tickersValues.GetValueOrDefault(brokerAccountSecurity.Security.Ticker);

                brokerAccountSecurity.CurrentPrice = value * brokerAccountSecurity.Quantity;
            }

            await _db.Commit();
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

        private Expression<Func<BrokerAccountSecurity, bool>> GetBaseFilter(Guid brokerAccountId)
        {
            return brokerAccountSecurity => brokerAccountSecurity.BrokerAccountId == brokerAccountId;
        }

        private IQueryable<BrokerAccountSecurity> GetFullHierarchyColumns(IQueryable<BrokerAccountSecurity> brokerAccountSecurityQuery)
        {
            return brokerAccountSecurityQuery
                .Include(brokerAccountSecurity => brokerAccountSecurity.Security.Type)
                .Include(brokerAccountSecurity => brokerAccountSecurity.BrokerAccount.Type)
                .Include(brokerAccountSecurity => brokerAccountSecurity.BrokerAccount.Currency)
                .Include(brokerAccountSecurity => brokerAccountSecurity.BrokerAccount.Broker);
        }
    }
}
