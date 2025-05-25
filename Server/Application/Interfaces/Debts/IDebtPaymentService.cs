using MoneyManager.Application.DTO.Currencies;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MoneyManager.Application.DTO.Debts;

namespace MoneyManager.Application.Interfaces.Debts
{
    public interface IDebtPaymentService
    {
        Task<IEnumerable<DebtPaymentDto>> GetAll();
        Task<Guid> Add(DebtPaymentDto debtPayment);
        Task Update(DebtPaymentDto debtPayment);
        Task Delete(Guid id);
    }
}