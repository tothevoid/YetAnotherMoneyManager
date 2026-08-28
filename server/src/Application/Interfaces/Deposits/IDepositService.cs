using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MoneyManager.Application.DTO.Deposits;

namespace MoneyManager.Application.Interfaces.Deposits
{
    public interface IDepositService
    {
        Task<IEnumerable<DepositDto>> GetAllAsync(int monthsFrom, int monthsTo, bool onlyActive);

        Task<IEnumerable<DepositDto>> GetAllActiveAsync();

        Task<Guid> AddAsync(DepositDto deposit);

        Task UpdateAsync(DepositDto modifiedDeposit);

        Task DeleteAsync(Guid id);

        Task<DepositMonthSummaryDto> GetSummaryAsync(int monthsFrom, int monthsTo, bool onlyActive);

        Task<DepositsRangeDto> GetDepositsRangeAsync();
    }
}
