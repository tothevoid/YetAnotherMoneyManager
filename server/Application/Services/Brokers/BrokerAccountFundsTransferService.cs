using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MoneyManager.Infrastructure.Entities.Brokers;
using MoneyManager.Infrastructure.Interfaces.Database;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using MoneyManager.Application.DTO.Brokers;
using MoneyManager.Application.Interfaces.Accounts;
using MoneyManager.Application.Interfaces.Brokers;

namespace MoneyManager.Application.Services.Brokers
{
    public class BrokerAccountFundsTransferService : IBrokerAccountFundsTransferService
    {
        private readonly IUnitOfWork _db;
        private readonly IRepository<BrokerAccountFundsTransfer> _repo;
        private readonly IBrokerAccountService _brokerAccountService;
        private readonly IAccountService _accountService;
        private readonly IMapper _mapper;

        public BrokerAccountFundsTransferService(IUnitOfWork uow, IMapper mapper, IBrokerAccountService brokerAccountService, IAccountService accountService)
        {
            _db = uow;
            _mapper = mapper;
            _repo = uow.CreateRepository<BrokerAccountFundsTransfer>();
            _brokerAccountService = brokerAccountService;
            _accountService = accountService;
        }

        public async Task<IEnumerable<BrokerAccountFundsTransferDto>> GetAllAsync(Guid brokerAccountId, bool income)
        {
            var transfers = await _repo.GetAll(null, GetFullHierarchyColumns);
            return _mapper.Map<IEnumerable<BrokerAccountFundsTransferDto>>(transfers)
                .ToList();
        }

        public async Task<BrokerAccountFundsTransferDto> Add(BrokerAccountFundsTransferDto transferDto)
        {
            var transfer = _mapper.Map<BrokerAccountFundsTransfer>(transferDto);

            transfer.Id = Guid.NewGuid();
            await _repo.Add(transfer);

            await UpdateLinkedAccountsBalance(transferDto.AccountId, transferDto.BrokerAccountId, 
                transferDto.Amount * (transferDto.Income ? 1: -1));

            await _db.Commit();
            return transferDto;
        }

        public async Task Update(BrokerAccountFundsTransferDto transferDto)
        {
            var storedTransfer = await _repo.GetById(transferDto.Id);

            var transfer = _mapper.Map<BrokerAccountFundsTransfer>(transferDto);
            _repo.Update(transfer);

            var amountBefore = storedTransfer.Amount * (storedTransfer.Income ? 1 : -1);
            var amountNow = transferDto.Amount * (transferDto.Income ? 1 : -1);

            var diff = (amountBefore - amountNow) * -1;
            await UpdateLinkedAccountsBalance(transferDto.AccountId, transferDto.BrokerAccountId, diff);

            await _db.Commit();
        }

        public async Task Delete(Guid id)
        {
            var transfer = await _repo.GetById(id);

            await _repo.Delete(id);

            await UpdateLinkedAccountsBalance(transfer.AccountId, transfer.BrokerAccountId,
                transfer.Amount * (transfer.Income ? -1 : 1));

            await _db.Commit();
        }

        public async Task UpdateLinkedAccountsBalance(Guid accountId, Guid brokerAccountId, decimal amount)
        {
            var brokerAccount = await _brokerAccountService.GetById(brokerAccountId, false);
            if (brokerAccount == null)
            {
                throw new ArgumentException(nameof(brokerAccountId));
            }

            var account = await _accountService.GetById(accountId, false);

            if (account == null)
            {
                throw new ArgumentException(nameof(accountId));
            }

            brokerAccount.MainCurrencyAmount += amount;
            account.Balance = -1 * amount;
        }

        private IQueryable<BrokerAccountFundsTransfer> GetFullHierarchyColumns(IQueryable<BrokerAccountFundsTransfer> query)
        {
            return query
                .Include(x => x.Account)
                .Include(x => x.BrokerAccount);
        }
    }
}
