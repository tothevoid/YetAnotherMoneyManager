using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using MoneyManager.Application.DTO.Securities;
using MoneyManager.Application.Interfaces.FileStorage;
using MoneyManager.Application.Interfaces.Integrations.Stock;
using MoneyManager.Application.Interfaces.Securities;
using MoneyManager.Application.Mappings;
using MoneyManager.Infrastructure.Entities.Brokers;
using MoneyManager.Infrastructure.Entities.Securities;
using MoneyManager.Infrastructure.Interfaces.Database;

namespace MoneyManager.Application.Services.Securities
{
    public class SecurityService(IUnitOfWork uow, ApplicationMapper mapper, IStockConnector stockConnector, 
        IFileStorageService fileStorageService) : ISecurityService
    {
        private readonly IUnitOfWork _db = uow;

        private readonly IRepository<Security> _securityRepo = uow.CreateRepository<Security>();
        private readonly IRepository<BrokerAccountSecurity> _brokerAccountSecurityRepo = uow.CreateRepository<BrokerAccountSecurity>();
        private readonly IRepository<SecurityTransaction> _securityTransactionsRepo = uow.CreateRepository<SecurityTransaction>();
        private readonly IRepository<DividendPayment> _dividendPaymentRepo = uow.CreateRepository<DividendPayment>();

        private readonly IStockConnector _stockConnector = stockConnector;
        private readonly ApplicationMapper _mapper = mapper;
        private readonly IFileStorageService _fileStorageService = fileStorageService;
        private const string _iconsBucket = "security";

        public async Task<IEnumerable<SecurityDTO>> GetAll(bool disableTracking = true)
        {
            var securities = await _securityRepo.GetAll(include: GetFullHierarchyColumns, disableTracking: disableTracking);
            return _mapper.Map(securities);
        }
        public async Task<SecurityDTO> FindByTicker(string ticker)
        {
            var securities = await _securityRepo
                .GetAll(filter: security => string.Equals(security.Ticker, ticker, StringComparison.OrdinalIgnoreCase), include: GetFullHierarchyColumns);
            return (await FindByTickers([ticker])).FirstOrDefault();
        }

        public async Task<IEnumerable<SecurityDTO>> FindByTickers(IEnumerable<string> tickers)
        {
            var lowerTickers = tickers.Select(ticker => ticker.ToLower()).ToArray();

            var securities = await _securityRepo
                .GetAll(filter: security => lowerTickers.Contains(security.Ticker.ToLower()), include: GetFullHierarchyColumns);
            return _mapper.Map(securities);
        }

        public async Task<SecurityStatsDto> GetStats(Guid securityId)
        {
            // TODO: compare performance between db calls and linq calls
            var securityTransactionsPrices = (await _securityTransactionsRepo.GetAll(transaction => transaction.SecurityId == securityId))
               .ToArray();

            if (securityTransactionsPrices.Length == 0)
            {
                return new SecurityStatsDto();
            }

            decimal min = securityTransactionsPrices[0].Price;
            decimal max = securityTransactionsPrices[0].Price;
            decimal pricesSum = 0;
            decimal totalSum = 0;

            foreach (var transaction in securityTransactionsPrices)
            {
                var transactionPrice = transaction.Price;

                if (transactionPrice < min)
                {
                    min = transactionPrice;
                }

                if (transactionPrice > max)
                {
                    max = transactionPrice;
                }

                pricesSum += transactionPrice;
                totalSum += transactionPrice * transaction.Quantity;
            }
        
            var hasOnBrokerAccounts = await _brokerAccountSecurityRepo.GetSum(brokerAccountSecurity => brokerAccountSecurity.Quantity,
                brokerAccountSecurity => brokerAccountSecurity.SecurityId == securityId);

            var dividendsIncome = await _dividendPaymentRepo.GetSum(dividendPayment => dividendPayment.SecuritiesQuantity * dividendPayment.Dividend.Amount - dividendPayment.Tax, 
                dividendPayment => dividendPayment.Dividend.SecurityId == securityId);

            return new SecurityStatsDto
            {
                TransactionsMin = min,
                TransactionsMax = max,
                TransactionsSum = totalSum,
                HasOnBrokerAccounts = (int)hasOnBrokerAccounts,
                //TODO: also calculate weighed mean
                TransactionsAvg = pricesSum / securityTransactionsPrices.Length,
                DividendsIncome = dividendsIncome
            };
        }

        public async Task<SecurityDTO> GetById(Guid id, bool loadHierarchy = true, bool disableTracking = true)
        {
            var security = await _securityRepo.GetById(id, loadHierarchy ? GetFullHierarchyColumns: null, disableTracking);
            var securityDto = _mapper.Map(security);
            return securityDto;
        }

        public async Task<IEnumerable<SecurityHistoryValueDto>> GetTickerHistory(string ticker)
        {
            var to = DateOnly.FromDateTime(DateTime.Now);
            var from = to.AddMonths(-3);

            var security = await FindByTicker(ticker);
                
            if (security == null)
            {
                return [];
            }

            return await _stockConnector.GetTickerHistory(security, from, to);
        }

        public async Task<SecurityDTO> Add(SecurityDTO securityDto, IFormFile securityIcon)
        {
            var security = _mapper.Map(securityDto);
            security.Id = Guid.NewGuid();
            
            if (securityIcon != null)
            {
                var key = security.Id.ToString();
                await _fileStorageService.UploadFile(_iconsBucket, securityIcon, key);
                security.IconKey = key;
            }

            await _securityRepo.Add(security);
            await _db.Commit();

            return await GetById(security.Id);
        }

        public async Task<SecurityDTO> Update(SecurityDTO securityTypeDto, IFormFile securityIcon)
        {
            var security = _mapper.Map(securityTypeDto);

            if (securityIcon != null)
            {
                var key = security.Id.ToString();
                await _fileStorageService.UploadFile(_iconsBucket, securityIcon, key);
                security.IconKey = key;
            }

            _securityRepo.Update(security);
            await _db.Commit();

            return await GetById(security.Id);
        }

        public async Task Delete(Guid id)
        {
            await _securityRepo.Delete(id);
            await _db.Commit();
        }

        public async Task<string> GetIconUrl(string iconKey)
        {
            return await _fileStorageService.GetFileUrl(_iconsBucket, iconKey);
        }

        private IQueryable<Security> GetFullHierarchyColumns(IQueryable<Security> securityQuery)
        {
            return securityQuery
                .Include(security => security.Type)
                .Include(security => security.Currency);
        }
    }
}
