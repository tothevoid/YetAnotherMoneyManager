using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MoneyManager.Application.DTO.Debts;

namespace MoneyManager.Application.Interfaces.Debts
{
    public interface IDebtService
    {
        Task<IEnumerable<DebtDto>> GetAllAsync(bool onlyActive);
        Task<DebtDto> GetByIdAsync(Guid id);
        Task<Guid> AddAsync(DebtDto debt);
        Task UpdateAsync(DebtDto debt);
        Task DeleteAsync(Guid id);
    }
}