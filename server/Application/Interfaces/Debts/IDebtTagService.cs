using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MoneyManager.Application.DTO.Debts;

namespace MoneyManager.Application.Interfaces.Debts
{
    public interface IDebtTagService
    {
        Task<IEnumerable<DebtTagDto>> GetAll();
        Task<IEnumerable<DebtTagStatsDto>> GetStats();
        Task<DebtTagDto> GetById(Guid id);
        Task<Guid> Add(DebtTagDto debtTag);
        Task Update(DebtTagDto debtTag);
        Task Delete(Guid id);
    }
}
