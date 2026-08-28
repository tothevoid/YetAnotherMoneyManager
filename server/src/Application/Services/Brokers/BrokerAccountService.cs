using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Audex.Application.DTO.Brokers;
using Audex.Application.Interfaces.Brokers;
using Audex.Application.Interfaces.Integrations.Stock;
using Audex.Application.Mappings;
using Audex.Infrastructure.Entities.Brokers;
using Audex.Infrastructure.Interfaces.Database;
using Audex.Infrastructure.Queries;

namespace Audex.Application.Services.Brokers
{
    public class BrokerAccountService : IBrokerAccountService
    {
        private readonly IUnitOfWork _db;

        private readonly IRepository<BrokerAccount> _brokerAccountRepo;
        private readonly IBrokerAccountSecurityService _brokerAccountSecurityService;

        private readonly ApplicationMapper _mapper;
       
        public BrokerAccountService(IUnitOfWork uow, ApplicationMapper mapper, 
            IBrokerAccountSecurityService brokerAccountSecurityService)
        {
            _db = uow;
            _mapper = mapper;
            _brokerAccountSecurityService = brokerAccountSecurityService;
            _brokerAccountRepo = uow.CreateRepository<BrokerAccount>();
        }

        public async Task<IEnumerable<BrokerAccountDto>> GetAllAsync()
        {
            var query = new ComplexQueryBuilder<BrokerAccount>()
                .AddOrder(brokerAccount => brokerAccount.Name)
                .AddJoins(GetFullHierarchyColumns)
                .GetQuery();

            var brokerAccounts = await _brokerAccountRepo
                .GetAllAsync(query);

            var brokerAccountsDtos = _mapper.Map(brokerAccounts)
                .ToList();

            return brokerAccountsDtos;
        }

        public async Task<BrokerAccountDto> GetByIdAsync(Guid id)
        {
            var brokerAccount = await _brokerAccountRepo.GetByIdAsync(id, GetFullHierarchyColumns);
            var brokerAccountDto = _mapper.Map(brokerAccount);
            return brokerAccountDto;
        }

        public async Task<Guid> AddAsync(BrokerAccountDto brokerAccountDto)
        {
            var brokerAccount = _mapper.Map(brokerAccountDto);
            brokerAccount.Id = Guid.NewGuid();
            await _brokerAccountRepo.AddAsync(brokerAccount);
            await _db.CommitAsync();
            return brokerAccount.Id;
        }

        public async Task UpdateAsync(BrokerAccountDto brokerAccountDto)
        {
            var brokerAccount = _mapper.Map(brokerAccountDto);
            _brokerAccountRepo.Update(brokerAccount);
            await _db.CommitAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            await _brokerAccountRepo.DeleteAsync(id);
            await _db.CommitAsync();
        }

        public async Task<decimal> GetTotalSoldAmountByBrokerAccountAsync(Guid brokerAccountId)
        {
            return await _brokerAccountSecurityService.GetTotalSoldByBrokerAccountAsync(brokerAccountId);
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
