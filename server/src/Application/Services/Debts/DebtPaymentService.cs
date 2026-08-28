using Microsoft.EntityFrameworkCore;
using MoneyManager.Application.DTO.Common;
using MoneyManager.Application.DTO.Debts;
using MoneyManager.Application.Interfaces.Debts;
using MoneyManager.Application.Interfaces.Transactions;
using MoneyManager.Application.Mappings;
using MoneyManager.Infrastructure.Entities.Accounts;
using MoneyManager.Infrastructure.Entities.Brokers;
using MoneyManager.Infrastructure.Entities.Debts;
using MoneyManager.Infrastructure.Interfaces.Database;
using MoneyManager.Infrastructure.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MoneyManager.Application.Services.Debts
{
    public class DebtPaymentService: IDebtPaymentService
    {
        private readonly IUnitOfWork _db;
        private readonly IRepository<Debt> _debtRepo;
        private readonly IRepository<DebtPayment> _debtPaymentRepo;
        private readonly IRepository<Account> _accountRepo;
        private readonly ApplicationMapper _mapper;

        public DebtPaymentService(IUnitOfWork uow, ApplicationMapper mapper)
        {
            _db = uow;
            _mapper = mapper;
            _debtRepo = uow.CreateRepository<Debt>();
            _debtPaymentRepo = uow.CreateRepository<DebtPayment>();
            _accountRepo = uow.CreateRepository<Account>();
        }

        public async Task<DebtPaymentDto> GetByIdAsync(Guid id)
        {
            var debtPayment = await _debtPaymentRepo.GetByIdAsync(id);
            return _mapper.Map(debtPayment);
        }

        public async Task<IEnumerable<DebtPaymentDto>> GetAllAsync(int pageIndex, int recordsQuantity, Guid? debtId = null, Guid? tagId = null)
        {
            var builder = new ComplexQueryBuilder<DebtPayment>();

            if (debtId.HasValue && debtId.Value != Guid.Empty)
            {
                builder.AddFilter(payment => payment.DebtId == debtId.Value);
            }

            if (tagId.HasValue && tagId.Value != Guid.Empty)
            {
                builder.AddFilter(payment => payment.Debt.DebtTags.Any(dt => dt.DebtTagId == tagId.Value));
            }

            var query = builder
                .AddPagination(pageIndex, recordsQuantity,
                    (payment) => payment.Date, true)
                .AddJoins(GetFullHierarchyColumns)
                .GetQuery();

            var debtPayments = await _debtPaymentRepo.GetAllAsync(query);
            return _mapper.Map(debtPayments);
        }

        public async Task<PaginationConfigDto> GetPaginationAsync(Guid? debtId = null, Guid? tagId = null)
        {
            int pageSize = 10;
            int recordsQuantity;

            var hasDebtId = debtId.HasValue && debtId.Value != Guid.Empty;
            var hasTagId = tagId.HasValue && tagId.Value != Guid.Empty;

            if (hasDebtId && hasTagId)
            {
                recordsQuantity = await _debtPaymentRepo.GetCountAsync(p => p.DebtId == debtId.Value && p.Debt.DebtTags.Any(dt => dt.DebtTagId == tagId.Value));
            }
            else if (hasDebtId)
            {
                recordsQuantity = await _debtPaymentRepo.GetCountAsync(p => p.DebtId == debtId.Value);
            }
            else if (hasTagId)
            {
                recordsQuantity = await _debtPaymentRepo.GetCountAsync(p => p.Debt.DebtTags.Any(dt => dt.DebtTagId == tagId.Value));
            }
            else
            {
                recordsQuantity = await _debtPaymentRepo.GetCountAsync();
            }

            return new PaginationConfigDto()
            {
                PageSize = pageSize,
                RecordsQuantity = recordsQuantity
            };
        }

        public async Task<Guid> AddAsync(DebtPaymentDto debtPaymentDto)
        {
            var debtPayment = _mapper.Map(debtPaymentDto);
            debtPayment.Id = Guid.NewGuid();

            await _debtPaymentRepo.AddAsync(debtPayment);

            await UpdateLinkedEntities(debtPayment.DebtId, debtPayment.TargetAccountId, debtPaymentDto.Amount, debtPaymentDto.IsPercentagePayment);
            await _db.CommitAsync();

            return debtPayment.Id;
        }

        public async Task UpdateAsync(DebtPaymentDto updatedPaymentDto)
        {
            var currentDebtPayment = await _debtPaymentRepo.GetByIdAsync(updatedPaymentDto.Id);
            var updatedDebtPayment = _mapper.Map(updatedPaymentDto);
            _debtPaymentRepo.Update(updatedDebtPayment);

            await ActualizeDebts(currentDebtPayment, updatedDebtPayment);
            await ActualizeAccounts(currentDebtPayment, updatedDebtPayment);

            await _db.CommitAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var debtPayment = await _debtPaymentRepo.GetByIdAsync(id);

            if (debtPayment == null)
            {
               return;
            }

            await _debtPaymentRepo.DeleteAsync(id);

            await UpdateLinkedEntities(debtPayment.DebtId, debtPayment.TargetAccountId, debtPayment.Amount * -1, 
                debtPayment.IsPercentagePayment);

            await _db.CommitAsync();
        }

        private async Task ActualizeDebts(DebtPayment currentDebtPayment, DebtPayment updatedDebtPayment)
        {
            var amountChanged = currentDebtPayment.Amount != updatedDebtPayment.Amount;
            var debtChanged = currentDebtPayment.DebtId != updatedDebtPayment.DebtId;
            var percentagePaymentChanged = currentDebtPayment.IsPercentagePayment != updatedDebtPayment.IsPercentagePayment;

            if (!amountChanged && !debtChanged && !percentagePaymentChanged)
            {
                return;
            }

            if (debtChanged)
            {
                await ResetPreviousDebtChanges(currentDebtPayment);
                await UpdateLinkedDebt(updatedDebtPayment.DebtId, -1 * updatedDebtPayment.Amount);
                return;
            }

            if (percentagePaymentChanged)
            {
                var diff = updatedDebtPayment.IsPercentagePayment
                    ? currentDebtPayment.Amount
                    : updatedDebtPayment.Amount * -1;

                await UpdateLinkedDebt(updatedDebtPayment.DebtId, diff);
            }
            else if (!updatedDebtPayment.IsPercentagePayment)
            {
                await UpdateLinkedDebt(updatedDebtPayment.DebtId, currentDebtPayment.Amount - updatedDebtPayment.Amount);
            }
        }

        private async Task ActualizeAccounts(DebtPayment currentDebtPayment, DebtPayment updatedDebtPayment)
        {
            if (currentDebtPayment.TargetAccountId != updatedDebtPayment.TargetAccountId)
            {
                await UpdateLinkedAccount(currentDebtPayment.TargetAccountId, currentDebtPayment.Amount * -1);
                await UpdateLinkedAccount(updatedDebtPayment.TargetAccountId, updatedDebtPayment.Amount);
            }
            else if (currentDebtPayment.Amount != updatedDebtPayment.Amount)
            {
                await UpdateLinkedAccount(updatedDebtPayment.TargetAccountId, updatedDebtPayment.Amount - currentDebtPayment.Amount);
            }
        }

        private async Task UpdateLinkedEntities(Guid debtId, Guid accountId, decimal diff, bool isPercentagePayment)
        {
            if (!isPercentagePayment)
            {
                await UpdateLinkedDebt(debtId, -1 * diff);
            }
            
            await UpdateLinkedAccount(accountId, diff);
        }

        private async Task UpdateLinkedDebt(Guid debtId, decimal diff)
        {
            var debt = await _debtRepo.GetByIdAsync(debtId, disableTracking: false);

            debt.Amount += diff;

            _debtRepo.Update(debt);
        }


        private async Task ResetPreviousDebtChanges(DebtPayment currentDebtPayment)
        {
            if (currentDebtPayment.IsPercentagePayment)
            {
                return;
            }

            var debt = await _debtRepo.GetByIdAsync(currentDebtPayment.DebtId, disableTracking: false);

            debt.Amount += currentDebtPayment.Amount;

            _debtRepo.Update(debt);
        }

        private async Task UpdateLinkedAccount(Guid accountId, decimal diff)
        {
            var account = await _accountRepo.GetByIdAsync(accountId, disableTracking: false);

            account.Balance += diff;

            _accountRepo.Update(account);
        }

        private IQueryable<DebtPayment> GetFullHierarchyColumns(IQueryable<DebtPayment> debtPaymentQuery)
        {
            return debtPaymentQuery
                .Include(debtPayment => debtPayment.Debt.Currency)
                .Include(debtPayment => debtPayment.TargetAccount.AccountType)
                .Include(debtPayment => debtPayment.TargetAccount.Currency)
                .Include(debtPayment => debtPayment.TargetAccount.Bank);
        }
    }
}
