using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MoneyManager.Application.DTO.Securities;
using MoneyManager.Application.Integrations.Stock;
using MoneyManager.Application.Interfaces.Integrations.Stock;
using MoneyManager.Application.Interfaces.Securities;
using MoneyManager.Infrastructure.Entities.Brokers;
using MoneyManager.Infrastructure.Entities.Securities;
using MoneyManager.Infrastructure.Interfaces.Database;
using MoneyManager.Infrastructure.Queries;

namespace MoneyManager.Application.Services.Securities
{
    public class SecurityTransactionService : ISecurityTransactionService
    {
        private readonly IUnitOfWork _db;

        private readonly IRepository<SecurityTransaction> _securityTransactionRepo;
        private readonly IRepository<BrokerAccountSecurity> _brokerAccountSecurityRepo;
        private readonly IRepository<BrokerAccount> _brokerAccountRepo;
        private readonly IMapper _mapper;

        public SecurityTransactionService(IUnitOfWork uow, IMapper mapper)
        {
            _db = uow;
            _mapper = mapper;
            _securityTransactionRepo = uow.CreateRepository<SecurityTransaction>();
            _brokerAccountSecurityRepo = uow.CreateRepository<BrokerAccountSecurity>();
            _brokerAccountRepo = uow.CreateRepository<BrokerAccount>();
        }

        public async Task<IEnumerable<SecurityTransactionDTO>> GetAll(Guid brokerAccountId,
            int recordsQuantity, int pageIndex)
        {
            var complexQuery = new ComplexQueryBuilder<SecurityTransaction>()
                .AddFilter(GetBaseFilter(brokerAccountId))
                .AddJoins(GetFullHierarchyColumns)
                .AddPagination(securityTransaction => securityTransaction.Date, recordsQuantity,
                    (pageIndex - 1) * recordsQuantity, true)
                .DisableTracking()
                .GetQuery();

            var brokerAccountSecurities = await _securityTransactionRepo
                .GetAll(complexQuery);
            return _mapper.Map<IEnumerable<SecurityTransactionDTO>>(brokerAccountSecurities);
        }

        public async Task<IEnumerable<SecurityTransactionDTO>> GetTransactionsBySecurity(Guid securityId)
        {
            var complexQuery = new ComplexQueryBuilder<SecurityTransaction>()
                .AddFilter(securityTransaction => securityTransaction.SecurityId == securityId)
                .AddJoins(GetFullHierarchyColumns)
                .AddOrder(securityTransaction => securityTransaction.Date)
                .DisableTracking()
                .GetQuery();

            var transactions = await _securityTransactionRepo
                .GetAll(complexQuery);

            return _mapper.Map<IEnumerable<SecurityTransactionDTO>>(transactions);
        }

        public async Task<SecurityTransactionPaginationDto> GetPagination(Guid brokerAccountId)
        {
            int pageSize = 20;
            var recordsQuantity = await _securityTransactionRepo.GetCount(GetBaseFilter(brokerAccountId));

            return new SecurityTransactionPaginationDto()
            {
                PageSize = pageSize,
                RecordsQuantity = recordsQuantity
            };
        }

        private Expression<Func<SecurityTransaction, bool>> GetBaseFilter(Guid brokerAccountId)
        {
            return transaction => transaction.BrokerAccountId == brokerAccountId;
        }

        public async Task<Guid> Add(SecurityTransactionDTO securityDto)
        {
            var securityTransaction = _mapper.Map<SecurityTransaction>(securityDto);
            securityTransaction.Id = Guid.NewGuid();
            await ApplyAddedTransaction(securityDto);
            await _securityTransactionRepo.Add(securityTransaction);
            await _db.Commit();
            return securityTransaction.Id;
        }

        public async Task Update(SecurityTransactionDTO securityDto)
        {
            await ApplyModifiedTransaction(securityDto);
            var securityTransaction = _mapper.Map<SecurityTransaction>(securityDto);
            _securityTransactionRepo.Update(securityTransaction);
            await _db.Commit();
        }

        public async Task Delete(Guid id)
        {
            //TODO: Add transaction
            await SubtractDeletedTransaction(id);
            await _securityTransactionRepo.Delete(id);

            await _db.Commit();
        }

        private async Task<BrokerAccountSecurity> FindExistingBrokerAccountSecurity(SecurityTransactionDTO securityTransaction)
        {
            return await _brokerAccountSecurityRepo.Find(brokerAccountSecurity =>
                brokerAccountSecurity.BrokerAccountId == securityTransaction.BrokerAccountId &&
                brokerAccountSecurity.SecurityId == securityTransaction.SecurityId);
        }

        private async Task GenerateBrokerAccountSecurity(SecurityTransactionDTO securityTransaction)
        {
            var price = securityTransaction.Price * securityTransaction.Quantity;
            var brokerAccountSecurity = new BrokerAccountSecurity()
            {
                SecurityId = securityTransaction.SecurityId,
                BrokerAccountId = securityTransaction.BrokerAccountId,
                Price = price,
                Quantity = securityTransaction.Quantity
            };

            await _brokerAccountSecurityRepo.Add(brokerAccountSecurity);
            await ActualizeBrokerAccountCurrencyValue(brokerAccountSecurity.BrokerAccountId, -1 * price);

            await _db.Commit();
        }

        private async Task ApplyAddedTransaction(SecurityTransactionDTO securityTransaction)
        {
            var brokerAccountSecurity = await FindExistingBrokerAccountSecurity(securityTransaction);
            if (brokerAccountSecurity != null)
            {
                var price = securityTransaction.Quantity * securityTransaction.Price;
                brokerAccountSecurity.Quantity += securityTransaction.Quantity;
                brokerAccountSecurity.Price += securityTransaction.Quantity * securityTransaction.Price;
                _brokerAccountSecurityRepo.Update(brokerAccountSecurity);

                await ActualizeBrokerAccountCurrencyValue(brokerAccountSecurity.BrokerAccountId, -1 * price);
                await _db.Commit();
            }
            else
            {
                await GenerateBrokerAccountSecurity(securityTransaction);
            }
        }

        private async Task ApplyModifiedTransaction(SecurityTransactionDTO modifiedSecurityTransaction)
        {
            var brokerAccountSecurity = await FindExistingBrokerAccountSecurity(modifiedSecurityTransaction);
            if (brokerAccountSecurity != null)
            {
                await ApplyTransactionChanges(brokerAccountSecurity, modifiedSecurityTransaction);
            }
            else
            {
                await GenerateBrokerAccountSecurity(modifiedSecurityTransaction);
            }
        }

        private async Task ApplyTransactionChanges(BrokerAccountSecurity brokerAccountSecurity,
            SecurityTransactionDTO modifiedSecurityTransaction)
        {
            var committedSecurityTransaction = await _securityTransactionRepo.GetById(modifiedSecurityTransaction.Id);
            var quantityDiff = modifiedSecurityTransaction.Quantity - committedSecurityTransaction.Quantity;

            if (quantityDiff == 0 &&
                committedSecurityTransaction.Price == modifiedSecurityTransaction.Quantity)
            {
                return;
            }

            if (quantityDiff != 0)
            {
                brokerAccountSecurity.Quantity += modifiedSecurityTransaction.Quantity - committedSecurityTransaction.Quantity;
            }

            var priceDiff = modifiedSecurityTransaction.Quantity * modifiedSecurityTransaction.Price -
                                   committedSecurityTransaction.Quantity * committedSecurityTransaction.Price;

            brokerAccountSecurity.Price += priceDiff;

            _brokerAccountSecurityRepo.Update(brokerAccountSecurity);
            await ActualizeBrokerAccountCurrencyValue(brokerAccountSecurity.BrokerAccountId, priceDiff * -1);

            await _db.Commit();
        }

        private async Task ActualizeBrokerAccountCurrencyValue(Guid brokerAccountId, decimal currencyDiff)
        {
            var brokerAccount = await _brokerAccountRepo.GetById(brokerAccountId, disableTracking: false);
            brokerAccount.MainCurrencyAmount += currencyDiff;
            _brokerAccountRepo.Update(brokerAccount);
        }

        private async Task SubtractDeletedTransaction(Guid transactionId)
        {
            var securityTransaction = await _securityTransactionRepo.GetById(transactionId);
            var securityTransactionDto = _mapper.Map<SecurityTransactionDTO>(securityTransaction);
            var brokerAccountSecurity = await FindExistingBrokerAccountSecurity(securityTransactionDto);

            if (brokerAccountSecurity == null)
            {
                return;
            }

            var price = securityTransaction.Quantity * securityTransaction.Price;
            brokerAccountSecurity.Quantity -= securityTransaction.Quantity;
            brokerAccountSecurity.Price -= price;
            _brokerAccountSecurityRepo.Update(brokerAccountSecurity);
            await ActualizeBrokerAccountCurrencyValue(brokerAccountSecurity.BrokerAccountId, price);

            await _db.Commit();
        }
        private IQueryable<SecurityTransaction> GetFullHierarchyColumns(
            IQueryable<SecurityTransaction> securityTransactionQuery)
        {
            return securityTransactionQuery
                .Include(security => security.BrokerAccount.Type)
                .Include(security => security.BrokerAccount.Currency)
                .Include(security => security.BrokerAccount.Broker)
                .Include(security => security.Security.Type)
                .Include(security => security.Security.Currency);
        }
    }
}
