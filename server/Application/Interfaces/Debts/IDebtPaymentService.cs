using MoneyManager.Application.DTO.Common;
using MoneyManager.Application.DTO.Debts;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MoneyManager.Application.Interfaces.Debts
{
    public interface IDebtPaymentService
    {
        Task<DebtPaymentDto> GetById(Guid id);
        Task<IEnumerable<DebtPaymentDto>> GetAll(int pageIndex, int recordsQuantity, Guid? debtId = null, Guid? tagId = null);
        Task<PaginationConfigDto> GetPagination(Guid? debtId = null, Guid? tagId = null);
        Task<Guid> Add(DebtPaymentDto debtPayment);
        Task Update(DebtPaymentDto updatedPaymentDto);
        Task Delete(Guid id);
    }
}