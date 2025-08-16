using MoneyManager.Application.DTO.Currencies;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MoneyManager.Application.DTO.Debts;

namespace MoneyManager.Application.Interfaces.Debts
{
    public interface IDebtService
    {
        Task<IEnumerable<DebtDto>> GetAll(bool onlyActive);
        Task<Guid> Add(DebtDto debt);
        Task Update(DebtDto debt);
        Task Delete(Guid id);
    }
}