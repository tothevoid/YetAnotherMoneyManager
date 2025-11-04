using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MoneyManager.Application.DTO.Accounts;
using MoneyManager.Application.DTO.Brokers;
using MoneyManager.Application.DTO.Common;
using MoneyManager.Application.Interfaces.Accounts;
using MoneyManager.Application.Interfaces.Brokers;
using MoneyManager.Infrastructure.Entities.Accounts;
using MoneyManager.Infrastructure.Entities.Brokers;
using MoneyManager.Infrastructure.Interfaces.Database;
using MoneyManager.Infrastructure.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MoneyManager.Application.Services.Brokers
{
    public class BrokerAccountFundsTransferService : IBrokerAccountFundsTransferService
    {
        private readonly IUnitOfWork _db;
        private readonly IRepository<BrokerAccountFundsTransfer> _transfersRepo;
        private readonly IRepository<Account> _accountRepo;
        private readonly IRepository<BrokerAccount> _brokerAccountRepo;
        private readonly IBrokerAccountService _brokerAccountService;
        private readonly IAccountService _accountService;
        private readonly IMapper _mapper;

        public BrokerAccountFundsTransferService(IUnitOfWork db, IMapper mapper, IBrokerAccountService brokerAccountService, IAccountService accountService)
        {
            _db = db;
            _mapper = mapper;
            _transfersRepo = db.CreateRepository<BrokerAccountFundsTransfer>();
            _accountRepo = db.CreateRepository<Account>();
            _brokerAccountRepo = db.CreateRepository<BrokerAccount>();
            _brokerAccountService = brokerAccountService;
            _accountService = accountService;
        }

        public async Task<IEnumerable<BrokerAccountFundsTransferDto>> GetAll(Guid brokerAccountId)
        {
            var complexQuery = GetBaseBuilder(brokerAccountId).GetQuery();

            var transfers = await _transfersRepo.GetAll(complexQuery);
            return _mapper.Map<IEnumerable<BrokerAccountFundsTransferDto>>(transfers).ToList();
        }

        public async Task<IEnumerable<BrokerAccountFundsTransferDto>> GetAll(Guid brokerAccountId, int pageIndex, int recordsQuantity)
        {
            var complexQuery = GetBaseBuilder(brokerAccountId)
                .AddPagination(pageIndex, recordsQuantity,
                    transfer => transfer.Date,
                    true)
                .GetQuery();

            var transfers = await _transfersRepo.GetAll(complexQuery);
            return _mapper.Map<IEnumerable<BrokerAccountFundsTransferDto>>(transfers).ToList();
        }

        private ComplexQueryBuilder<BrokerAccountFundsTransfer> GetBaseBuilder(Guid brokerAccountId)
        {
            return new ComplexQueryBuilder<BrokerAccountFundsTransfer>()
                .AddFilter(GetBaseFilter(brokerAccountId))
                .AddJoins(GetFullHierarchyColumns)
                .DisableTracking();
        }

        public async Task<BrokerAccountFundsTransferDto> Add(BrokerAccountFundsTransferDto transferDto)
        {
            var transfer = _mapper.Map<BrokerAccountFundsTransfer>(transferDto);
            await _transfersRepo.Add(transfer);

            await UpdateLinkedAccountsBalance(transferDto.AccountId, transferDto.BrokerAccountId, 
                transferDto.Amount * (transferDto.Income ? 1: -1));

            await _db.Commit();
            
            var storedRecord = await _transfersRepo.GetById(transfer.Id, GetFullHierarchyColumns);
            return _mapper.Map<BrokerAccountFundsTransferDto>(storedRecord);
        }

        public async Task Update(BrokerAccountFundsTransferDto transferDto)
        {
            var storedTransfer = await _transfersRepo.GetById(transferDto.Id);

            var transfer = _mapper.Map<BrokerAccountFundsTransfer>(transferDto);
            _transfersRepo.Update(transfer);

            var amountBefore = storedTransfer.Amount * (storedTransfer.Income ? 1 : -1);
            var amountNow = transferDto.Amount * (transferDto.Income ? 1 : -1);

            var diff = (amountBefore - amountNow) * -1;
            await UpdateLinkedAccountsBalance(transferDto.AccountId, transferDto.BrokerAccountId, diff);

            await _db.Commit();
        }

        public async Task Delete(Guid id)
        {
            var transfer = await _transfersRepo.GetById(id);

            await _transfersRepo.Delete(id);

            await UpdateLinkedAccountsBalance(transfer.AccountId, transfer.BrokerAccountId,
                transfer.Amount * (transfer.Income ? -1 : 1));

            await _db.Commit();
        }

        public async Task UpdateLinkedAccountsBalance(Guid accountId, Guid brokerAccountId, decimal amount)
        {
            var brokerAccount = await _brokerAccountRepo.GetById(brokerAccountId);
            if (brokerAccount == null)
            {
                throw new ArgumentException(nameof(brokerAccountId));
            }

            var account = await _accountRepo.GetById(accountId);

            if (account == null)
            {
                throw new ArgumentException(nameof(accountId));
            }

            brokerAccount.MainCurrencyAmount += amount;
            await _brokerAccountService.Update(_mapper.Map<BrokerAccountDTO>(brokerAccount));
            account.Balance += -1 * amount;
            await _accountService.Update(_mapper.Map<AccountDTO>(account));
        }

        public async Task<PaginationConfigDto> GetPagination(Guid brokerAccountId)
        {
            int pageSize = 10;
            var recordsQuantity = await _transfersRepo.GetCount(GetBaseFilter(brokerAccountId));

            return new PaginationConfigDto()
            {
                PageSize = pageSize,
                RecordsQuantity = recordsQuantity
            };
        }

        private Expression<Func<BrokerAccountFundsTransfer, bool>> GetBaseFilter(Guid brokerAccountId)
        {
            return brokerAccountSecurity => brokerAccountSecurity.BrokerAccountId == brokerAccountId;
        }

        private IQueryable<BrokerAccountFundsTransfer> GetFullHierarchyColumns(IQueryable<BrokerAccountFundsTransfer> query)
        {
            return query
                .Include(x => x.Account.Currency)
                .Include(x => x.Account.AccountType)
                .Include(x => x.Account.Bank)
                .Include(x => x.BrokerAccount.Type)
                .Include(x => x.BrokerAccount.Currency)
                .Include(x => x.BrokerAccount.Broker)
                .Include(x => x.BrokerAccount.Bank);
        }
    }
}
