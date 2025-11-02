using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MoneyManager.Infrastructure.Entities.Brokers;
using MoneyManager.Infrastructure.Interfaces.Database;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using MoneyManager.Application.DTO.Accounts;
using MoneyManager.Application.DTO.Brokers;
using MoneyManager.Application.Interfaces.Accounts;
using MoneyManager.Application.Interfaces.Brokers;
using MoneyManager.Infrastructure.Entities.Accounts;
using MoneyManager.Infrastructure.Queries;

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

        public BrokerAccountFundsTransferService(IUnitOfWork uow, IMapper mapper, IBrokerAccountService brokerAccountService, IAccountService accountService)
        {
            _db = uow;
            _mapper = mapper;
            _transfersRepo = uow.CreateRepository<BrokerAccountFundsTransfer>();
            _accountRepo = uow.CreateRepository<Account>();
            _brokerAccountRepo = uow.CreateRepository<BrokerAccount>();
            _brokerAccountService = brokerAccountService;
            _accountService = accountService;
        }

        public async Task<IEnumerable<BrokerAccountFundsTransferDto>> GetAllAsync(Guid brokerAccountId)
        {
            var complexQuery = new ComplexQueryBuilder<BrokerAccountFundsTransfer>()
                .AddFilter(transfer => transfer.BrokerAccountId == brokerAccountId)
                .AddJoins(GetFullHierarchyColumns)
                .AddOrder(transfer => transfer.Date, true)
                .DisableTracking()
                .GetQuery();

            var transfers = await _transfersRepo.GetAll(complexQuery);
            return _mapper.Map<IEnumerable<BrokerAccountFundsTransferDto>>(transfers).ToList();
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
            account.Balance = -1 * amount;
            await _accountService.Update(_mapper.Map<AccountDTO>(account));
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
