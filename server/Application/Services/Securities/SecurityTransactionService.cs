using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MoneyManager.Application.DTO.Common;
using MoneyManager.Application.DTO.Securities;
using MoneyManager.Application.Interfaces.Securities;
using MoneyManager.Application.Queries.Brokers;
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
        private readonly IRepository<DividendPayment> _dividendPaymentRepo;
        private readonly IMapper _mapper;

        public SecurityTransactionService(IUnitOfWork uow, IMapper mapper)
        {
            _db = uow;
            _mapper = mapper;
            _securityTransactionRepo = uow.CreateRepository<SecurityTransaction>();
            _brokerAccountSecurityRepo = uow.CreateRepository<BrokerAccountSecurity>();
            _brokerAccountRepo = uow.CreateRepository<BrokerAccount>();
            _dividendPaymentRepo = uow.CreateRepository<DividendPayment>();
        }

        public async Task<IEnumerable<SecurityTransactionDTO>> GetAll(Guid? brokerAccountId,
            int recordsQuantity, int pageIndex)
        {
            var query = new ComplexQueryBuilder<SecurityTransaction>()
                .AddJoins(GetFullHierarchyColumns)
                .AddPagination(pageIndex, recordsQuantity,
                    securityTransaction => securityTransaction.Date,
                    true)
                .DisableTracking();

            if (brokerAccountId != null)
            {
                query.AddFilter(GetBaseFilter((Guid) brokerAccountId));
            }

            var brokerAccountSecurities = await _securityTransactionRepo
                .GetAll(query.GetQuery());
            return _mapper.Map<IEnumerable<SecurityTransactionDTO>>(brokerAccountSecurities);
        }

        public async Task<IEnumerable<SecurityTransactionsHistoryDto>> GetTransactionsHistory(Guid securityId)
        {
            var complexQuery = new ComplexQueryBuilder<SecurityTransaction>()
                .AddFilter(securityTransaction => securityTransaction.SecurityId == securityId)
                .AddJoins(GetFullHierarchyColumns)
                .AddOrder(securityTransaction => securityTransaction.Date)
                .DisableTracking()
                .GetQuery();

            var transactions = await _securityTransactionRepo
                .GetAll(complexQuery);

            var dividendsPayments = (await _dividendPaymentRepo
                .GetAll(dividendPayment => dividendPayment.Dividend.SecurityId == securityId, DividendPaymentQuery.GetFullHierarchyColumns))
                .OrderBy(dividendPayment => dividendPayment.ReceivedAt)
                .ToArray();

            var convertedTransactions = new List<SecurityTransactionsHistoryDto>();

            //TODO: include sold securities
            foreach (var transaction in transactions)
            {
                var date = DateOnly.FromDateTime(transaction.Date);

                var paymentsSum = dividendsPayments
                    .Where(payment => payment.Dividend.SnapshotDate > date)
                    .Sum(payment => payment.Dividend.Amount - payment.Tax / payment.SecuritiesQuantity);

                convertedTransactions.Add(new SecurityTransactionsHistoryDto()
                {
                    Date = transaction.Date,
                    ValueWithPayments = transaction.Price - paymentsSum,
                    ValueWithoutPayments = transaction.Price,
                    Volume = transaction.Quantity
                });
            }

            return convertedTransactions;
        }

        public async Task<PaginationConfigDto> GetPagination(Guid brokerAccountId)
        {
            var filter = GetBaseFilter(brokerAccountId);
            return await GetFilteredPagination(filter);
        }

        public async Task<PaginationConfigDto> GetPagination()
        {
            return await GetFilteredPagination();
        }

        private async Task<PaginationConfigDto> GetFilteredPagination(Expression<Func<SecurityTransaction, bool>> filter = null)
        {
            int pageSize = 10;
            var recordsQuantity = await _securityTransactionRepo.GetCount(filter);

            return new PaginationConfigDto()
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
            await HandleAddedTransaction(securityDto);
            await _securityTransactionRepo.Add(securityTransaction);
            await _db.Commit();
            return securityTransaction.Id;
        }

        public async Task Update(SecurityTransactionDTO securityDto)
        {
            await HandleModifiedTransaction(securityDto);
            var securityTransaction = _mapper.Map<SecurityTransaction>(securityDto);
            _securityTransactionRepo.Update(securityTransaction);
            await _db.Commit();
        }

        public async Task Delete(Guid id)
        {
            await HandleDeletedTransaction(id);
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
            var price = securityTransaction.GetTotalPrice;
            var brokerAccountSecurity = new BrokerAccountSecurity()
            {
                SecurityId = securityTransaction.SecurityId,
                BrokerAccountId = securityTransaction.BrokerAccountId,
                Price = price,
                Quantity = securityTransaction.Quantity
            };

            await _brokerAccountSecurityRepo.Add(brokerAccountSecurity);
            await ActualizeBrokerAccountCurrencyValue(brokerAccountSecurity.BrokerAccountId, -1 * price);
        }

        private async Task HandleAddedTransaction(SecurityTransactionDTO securityTransaction)
        {
            var brokerAccountSecurity = await FindExistingBrokerAccountSecurity(securityTransaction);
            
            if (securityTransaction.IsSell)
            {
                await ApplyAddedSellTransaction(securityTransaction, brokerAccountSecurity);
            }
            else
            {
                await ApplyAddedBuyTransaction(securityTransaction, brokerAccountSecurity);
            }
        }

        private async Task ApplyAddedBuyTransaction(SecurityTransactionDTO securityTransaction, BrokerAccountSecurity brokerAccountSecurity)
        {
            if (brokerAccountSecurity != null)
            {
                var totalPrice = securityTransaction.GetTotalPrice;
                brokerAccountSecurity.Quantity += securityTransaction.Quantity;
                brokerAccountSecurity.Price += totalPrice;
                _brokerAccountSecurityRepo.Update(brokerAccountSecurity);

                await ActualizeBrokerAccountCurrencyValue(brokerAccountSecurity.BrokerAccountId, -1 * totalPrice);
            }
            else
            {
                await GenerateBrokerAccountSecurity(securityTransaction);
            }

        }

        private async Task ApplyAddedSellTransaction(SecurityTransactionDTO securityTransaction, BrokerAccountSecurity brokerAccountSecurity)
        {
            if (brokerAccountSecurity == null)
            {
                return;
            }

            var totalPrice = securityTransaction.GetTotalPrice;

            brokerAccountSecurity.SoldPrice += totalPrice;
            brokerAccountSecurity.SoldQuantity += securityTransaction.Quantity;

            _brokerAccountSecurityRepo.Update(brokerAccountSecurity);

            await ActualizeBrokerAccountCurrencyValue(brokerAccountSecurity.BrokerAccountId, totalPrice);
        }

        private async Task HandleModifiedTransaction(SecurityTransactionDTO modifiedSecurityTransaction)
        {
            var brokerAccountSecurity = await FindExistingBrokerAccountSecurity(modifiedSecurityTransaction);
            if (brokerAccountSecurity != null)
            {
                await ApplyTransactionChanges(brokerAccountSecurity, modifiedSecurityTransaction);
            }
        }

        private async Task ApplyTransactionChanges(BrokerAccountSecurity brokerAccountSecurity,
            SecurityTransactionDTO modifiedSecurityTransaction)
        {
            var committedSecurityTransaction = await _securityTransactionRepo.GetById(modifiedSecurityTransaction.Id);
            var committedSecurityTransactionDto = _mapper.Map<SecurityTransactionDTO>(committedSecurityTransaction);

            if (await HandleChangedBrokerAccount(committedSecurityTransactionDto, modifiedSecurityTransaction))
            {
                return;
            }

            if (await HandleChangedTransactionOperation(committedSecurityTransactionDto, modifiedSecurityTransaction))
            {
                return;
            }

            var quantityDiff = modifiedSecurityTransaction.Quantity - committedSecurityTransaction.Quantity;

            var totalCommittedPrice = committedSecurityTransactionDto.GetTotalPrice;
            var totalModifiedPrice = modifiedSecurityTransaction.GetTotalPrice;

            if (quantityDiff == 0 && totalCommittedPrice == totalModifiedPrice)
            {
                return;
            }

            if (quantityDiff != 0)
            {
                var diff = modifiedSecurityTransaction.Quantity - committedSecurityTransactionDto.Quantity;

                if (modifiedSecurityTransaction.IsSell)
                {
                    brokerAccountSecurity.SoldQuantity += diff;
                }
                else
                {
                    brokerAccountSecurity.Quantity += diff;
                }
            }

            var priceDiff = totalModifiedPrice - totalCommittedPrice;

            if (modifiedSecurityTransaction.IsSell)
            {
                brokerAccountSecurity.SoldPrice += priceDiff;
            }
            else
            {
                brokerAccountSecurity.Price += priceDiff;
            }

            _brokerAccountSecurityRepo.Update(brokerAccountSecurity);

            await ActualizeBrokerAccountCurrencyValue(brokerAccountSecurity.BrokerAccountId, priceDiff * -1);
        }

        private async Task<bool> HandleChangedBrokerAccount(SecurityTransactionDTO committedTransaction, 
            SecurityTransactionDTO modifiedTransaction)
        {
            if (committedTransaction.BrokerAccountId == modifiedTransaction.BrokerAccountId)
            {
                return false;
            }

            await HandleDeletedTransaction(committedTransaction.Id);
            await HandleAddedTransaction(modifiedTransaction);

            return true;
        }

        private async Task<bool> HandleChangedTransactionOperation(SecurityTransactionDTO committedTransaction,
            SecurityTransactionDTO modifiedTransaction)
        {
            if (committedTransaction.IsSell == modifiedTransaction.IsSell)
            {
                return false;
            }

            await HandleDeletedTransaction(committedTransaction.Id);
            await HandleAddedTransaction(modifiedTransaction);

            return true;
        }

        private async Task ActualizeBrokerAccountCurrencyValue(Guid brokerAccountId, decimal currencyDiff)
        {
            var brokerAccount = await _brokerAccountRepo.GetById(brokerAccountId, disableTracking: false);
            brokerAccount.MainCurrencyAmount += currencyDiff;
            _brokerAccountRepo.Update(brokerAccount);
        }

        private async Task HandleDeletedTransaction(Guid transactionId)
        {
            var securityTransaction = await _securityTransactionRepo.GetById(transactionId);
            var securityTransactionDto = _mapper.Map<SecurityTransactionDTO>(securityTransaction);
            var brokerAccountSecurity = await FindExistingBrokerAccountSecurity(securityTransactionDto);

            if (securityTransaction.IsSell)
            {
                await HandleDeletedSellTransaction(securityTransactionDto, brokerAccountSecurity);
            }
            else
            {
                await HandleDeletedBuyTransaction(securityTransactionDto, brokerAccountSecurity);
            }            
        }
        
        private async Task HandleDeletedBuyTransaction(SecurityTransactionDTO transaction, BrokerAccountSecurity brokerAccountSecurity)
        {
            if (brokerAccountSecurity == null)
            {
                return;
            }

            var price = transaction.GetTotalPrice;

            if (brokerAccountSecurity.Quantity == transaction.Quantity && brokerAccountSecurity.SoldQuantity == 0)
            {
                await _brokerAccountSecurityRepo.Delete(brokerAccountSecurity.Id);
            }
            else
            {
                brokerAccountSecurity.Quantity -= transaction.Quantity;
                brokerAccountSecurity.Price -= price;
                _brokerAccountSecurityRepo.Update(brokerAccountSecurity);
            }
           
            await ActualizeBrokerAccountCurrencyValue(brokerAccountSecurity.BrokerAccountId, price);
        }

        private async Task HandleDeletedSellTransaction(SecurityTransactionDTO transaction, BrokerAccountSecurity brokerAccountSecurity)
        {
            var price = transaction.GetTotalPrice;

            if (brokerAccountSecurity == null)
            {
                await GenerateBrokerAccountSecurity(transaction);
            }
            else
            {
                brokerAccountSecurity.SoldPrice += price;
                brokerAccountSecurity.SoldQuantity += transaction.Quantity;

                _brokerAccountSecurityRepo.Update(brokerAccountSecurity);
            }
           
            await ActualizeBrokerAccountCurrencyValue(transaction.BrokerAccountId, -1 * price);
        }

        private IQueryable<SecurityTransaction> GetFullHierarchyColumns(
            IQueryable<SecurityTransaction> securityTransactionQuery)
        {
            return securityTransactionQuery
                .Include(security => security.BrokerAccount.Type)
                .Include(security => security.BrokerAccount.Currency)
                .Include(security => security.BrokerAccount.Broker)
                .Include(security => security.BrokerAccount.Bank)
                .Include(security => security.Security.Type)
                .Include(security => security.Security.Currency);
        }
    }
}
