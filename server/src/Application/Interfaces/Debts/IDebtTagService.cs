using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Audex.Application.DTO.Debts;

namespace Audex.Application.Interfaces.Debts
{
    public interface IDebtTagService
    {
        Task<IEnumerable<DebtTagDto>> GetAllAsync();
        Task<IEnumerable<DebtTagStatsDto>> GetStatsAsync();
        Task<DebtTagDto> GetByIdAsync(Guid id);
        Task<Guid> AddAsync(DebtTagDto debtTag);
        Task UpdateAsync(DebtTagDto debtTag);
        Task DeleteAsync(Guid id);
        Task AssignTagsToDebtAsync(Guid debtId, IEnumerable<Guid> tagIds);
    }
}
