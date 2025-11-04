using MoneyManager.Application.DTO.Common;
using MoneyManager.Application.DTO.Currencies;
using MoneyManager.Application.DTO.Debts;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace MoneyManager.Application.Interfaces.Debts
{
    public interface IDebtPaymentService
    {
        Task<IEnumerable<DebtPaymentDto>> GetAll(int pageIndex, int recordsQuantity);
        Task<PaginationConfigDto> GetPagination();

        Task<Guid> Add(DebtPaymentDto debtPayment);
        Task Update(DebtPaymentDto updatedPaymentDto);
        Task Delete(Guid id);
    }
}