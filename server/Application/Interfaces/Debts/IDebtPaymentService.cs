using MoneyManager.Application.DTO.Common;
using MoneyManager.Application.DTO.Debts;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MoneyManager.Application.Interfaces.Debts
{
    public interface IDebtPaymentService
    {
        Task<DebtPaymentDto> GetByIdAsync(Guid id);
        Task<IEnumerable<DebtPaymentDto>> GetAllAsync(int pageIndex, int recordsQuantity, Guid? debtId = null, Guid? tagId = null);
        Task<PaginationConfigDto> GetPaginationAsync(Guid? debtId = null, Guid? tagId = null);
        Task<Guid> AddAsync(DebtPaymentDto debtPayment);
        Task UpdateAsync(DebtPaymentDto updatedPaymentDto);
        Task DeleteAsync(Guid id);
    }
}