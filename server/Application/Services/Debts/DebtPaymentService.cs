using AutoMapper;
using MoneyManager.Infrastructure.Interfaces.Database;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MoneyManager.Application.DTO.Debts;
using MoneyManager.Application.Interfaces.Debts;
using MoneyManager.Application.Interfaces.Transactions;
using MoneyManager.Infrastructure.Entities.Accounts;
using MoneyManager.Infrastructure.Entities.Debts;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using MoneyManager.Infrastructure.Entities.Brokers;
using MoneyManager.Infrastructure.Queries;

namespace MoneyManager.Application.Services.Debts
{
    public class DebtPaymentService: IDebtPaymentService
    {
        private readonly IUnitOfWork _db;
        private readonly IRepository<Debt> _debtRepo;
        private readonly IRepository<DebtPayment> _debtPaymentRepo;
        private readonly IRepository<Account> _accountRepo;
        private readonly IMapper _mapper;

        public DebtPaymentService(IUnitOfWork uow, IMapper mapper, ITransactionsService transactionService)
        {
            _db = uow;
            _mapper = mapper;
            _debtRepo = uow.CreateRepository<Debt>();
            _debtPaymentRepo = uow.CreateRepository<DebtPayment>();
            _accountRepo = uow.CreateRepository<Account>();
        }

        public async Task<IEnumerable<DebtPaymentDto>> GetAll()
        {
            var query = new ComplexQueryBuilder<DebtPayment>()
                .AddOrder(debtPayment => debtPayment.Date, true)
                .AddJoins(GetFullHierarchyColumns)
                .GetQuery();

            var debtPayments = await _debtPaymentRepo.GetAll(query);
            return _mapper.Map<IEnumerable<DebtPaymentDto>>(debtPayments);
        }

        public async Task<Guid> Add(DebtPaymentDto debtPaymentDto)
        {
            var debtPayment = _mapper.Map<DebtPayment>(debtPaymentDto);
            debtPayment.Id = Guid.NewGuid();

            await _debtPaymentRepo.Add(debtPayment);

            await UpdateLinkedEntities(debtPayment.DebtId, debtPayment.TargetAccountId, debtPaymentDto.Amount);
            await _db.Commit();

            return debtPayment.Id;
        }

        public async Task Update(DebtPaymentDto updatedPaymentDto)
        {
            var currentDebtPayment = await _debtPaymentRepo.GetById(updatedPaymentDto.Id);
            var updatedDebtPayment = _mapper.Map<DebtPayment>(updatedPaymentDto);
            _debtPaymentRepo.Update(updatedDebtPayment);

            var amountChanged = currentDebtPayment.Amount != updatedDebtPayment.Amount;

            // debt changed
            if (currentDebtPayment.DebtId != updatedDebtPayment.DebtId)
            {
                await UpdateLinkedDebt(currentDebtPayment.DebtId, currentDebtPayment.Amount);
                await UpdateLinkedDebt(updatedDebtPayment.DebtId, -1  * updatedDebtPayment.Amount);
            }
            else if (amountChanged)
            {
                await UpdateLinkedDebt(updatedDebtPayment.DebtId, currentDebtPayment.Amount - updatedDebtPayment.Amount);
            }

            // account changed
            if (currentDebtPayment.TargetAccountId != updatedDebtPayment.TargetAccountId)
            {
                await UpdateLinkedAccount(currentDebtPayment.TargetAccountId, currentDebtPayment.Amount * -1);
                await UpdateLinkedAccount(updatedDebtPayment.TargetAccountId, updatedDebtPayment.Amount);
            }
            else if (amountChanged)
            {
                await UpdateLinkedDebt(updatedDebtPayment.DebtId, updatedDebtPayment.Amount - currentDebtPayment.Amount);
            }

            await _db.Commit();
        }

        public async Task Delete(Guid id)
        {
            var debtPayment = await _debtPaymentRepo.GetById(id);

            if (debtPayment == null)
            {
               return;
            }

            await _debtPaymentRepo.Delete(id);

            await UpdateLinkedEntities(debtPayment.DebtId, debtPayment.TargetAccountId, debtPayment.Amount * -1);

            await _db.Commit();
        }

        private async Task UpdateLinkedEntities(Guid debtId, Guid accountId, decimal diff)
        {
            await UpdateLinkedDebt(debtId, -1 * diff);
            await UpdateLinkedAccount(accountId, diff);
        }

        private async Task UpdateLinkedDebt(Guid debtId, decimal diff)
        {
            var debt = await _debtRepo.GetById(debtId, disableTracking: false);

            debt.Amount += diff;

            _debtRepo.Update(debt);
        }

        private async Task UpdateLinkedAccount(Guid accountId, decimal diff)
        {
            var account = await _accountRepo.GetById(accountId, disableTracking: false);

            account.Balance += diff;

            _accountRepo.Update(account);
        }

        private IQueryable<DebtPayment> GetFullHierarchyColumns(IQueryable<DebtPayment> debtPaymentQuery)
        {
            return debtPaymentQuery
                .Include(debtPayment => debtPayment.Debt.Currency)
                .Include(debtPayment => debtPayment.TargetAccount.AccountType)
                .Include(debtPayment => debtPayment.TargetAccount.Currency);
        }
    }
}
