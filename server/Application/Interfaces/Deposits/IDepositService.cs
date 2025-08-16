using MoneyManager.Application.DTO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MoneyManager.Application.DTO.Deposits;

namespace MoneyManager.Application.Interfaces.Deposits
{
    public interface IDepositService
    {
        Task<IEnumerable<DepositDTO>> GetAll(int monthsFrom, int monthsTo, bool onlyActive);

        Task<IEnumerable<DepositDTO>> GetAllActive();

        Task<Guid> Add(DepositDTO deposit);

        Task Update(DepositDTO modifiedDeposit);

        Task Delete(Guid id);

        Task<DepositMonthSummaryDTO> GetSummary(int monthsFrom, int monthsTo, bool onlyActive);

        Task<DepositsRangeDTO> GetDepositsRange();
    }
}
