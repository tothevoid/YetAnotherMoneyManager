using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using MoneyManager.Application.DTO.Securities;
using MoneyManager.Application.Interfaces.Securities;
using MoneyManager.Infrastructure.Entities.Brokers;
using MoneyManager.Infrastructure.Entities.Securities;
using MoneyManager.Infrastructure.Interfaces.Database;

namespace MoneyManager.Application.Services.Securities
{
    public class SecurityTransactionService : ISecurityTransactionService
    {
        private readonly IUnitOfWork _db;

        private readonly IRepository<SecurityTransaction> _securityTransactionRepo;
        private readonly IRepository<BrokerAccountSecurity> _brokerAccountSecurityRepo;
        private readonly IMapper _mapper;
        public SecurityTransactionService(IUnitOfWork uow, IMapper mapper)
        {
            _db = uow;
            _mapper = mapper;
            _securityTransactionRepo = uow.CreateRepository<SecurityTransaction>();
            _brokerAccountSecurityRepo = uow.CreateRepository<BrokerAccountSecurity>();
        }

        public async Task<IEnumerable<SecurityTransactionDTO>> GetAll()
        {
            var securityTransactions = await _securityTransactionRepo.GetAll();
            return _mapper.Map<IEnumerable<SecurityTransactionDTO>>(securityTransactions);
        }

        public async Task<Guid> Add(SecurityTransactionDTO securityDto)
        {
            var securityTransaction = _mapper.Map<SecurityTransaction>(securityDto);
            securityTransaction.Id = Guid.NewGuid();
            await ActualizeBrokerAccount(securityDto);
            await _securityTransactionRepo.Add(securityTransaction);
            _db.Commit();
            return securityTransaction.Id;
        }
        public async Task Update(SecurityTransactionDTO securityDto)
        {
            var securityTransaction = _mapper.Map<SecurityTransaction>(securityDto);
            await ActualizeBrokerAccount(securityDto);
            await _securityTransactionRepo.Update(securityTransaction);
            _db.Commit();
        }

        public async Task Delete(Guid id)
        {
            var security = await _securityTransactionRepo.GetById(id);
            var securityDto = _mapper.Map<SecurityTransactionDTO>(security);

            await Task.WhenAll(
                ActualizeBrokerAccount(securityDto), 
                _securityTransactionRepo.Delete(id));
            _db.Commit();
        }

        private async Task ActualizeBrokerAccount(SecurityTransactionDTO securityTransaction)
        {
            var brokerAccountSecurity = await FindExistingBrokerAccountSecurity(securityTransaction);

            if (brokerAccountSecurity == null)
            {
                await GenerateBrokerAccountSecurity(securityTransaction);
            }
            else
            {
                await ModifyExistingBrokerAccountSecurity(brokerAccountSecurity, securityTransaction);
            }
        }

        private async Task ModifyExistingBrokerAccountSecurity(BrokerAccountSecurity brokerAccountSecurity,
            SecurityTransactionDTO securityTransaction)
        {
            brokerAccountSecurity.Quantity += securityTransaction.Quantity;
            brokerAccountSecurity.InitialPrice += securityTransaction.Price;

            await _brokerAccountSecurityRepo.Update(brokerAccountSecurity);
            _db.Commit();
        }

        private async Task<BrokerAccountSecurity> FindExistingBrokerAccountSecurity(SecurityTransactionDTO securityTransaction)
        {
            return await _brokerAccountSecurityRepo.Find(brokerAccountSecurity =>
                brokerAccountSecurity.BrokerAccountId == securityTransaction.BrokerAccountId &&
                brokerAccountSecurity.SecurityId == securityTransaction.SecurityId);
        }

        private async Task GenerateBrokerAccountSecurity(SecurityTransactionDTO securityTransaction)
        {
            var brokerAccountSecurity = new BrokerAccountSecurity()
            {
                SecurityId = securityTransaction.SecurityId,
                BrokerAccountId = securityTransaction.BrokerAccountId,
                InitialPrice = securityTransaction.Price,
                Quantity = securityTransaction.Quantity
            };

            await _brokerAccountSecurityRepo.Add(brokerAccountSecurity);
            _db.Commit();
        }
    }
}
