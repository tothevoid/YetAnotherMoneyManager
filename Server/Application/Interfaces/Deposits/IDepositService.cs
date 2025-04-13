using MoneyManager.BLL.DTO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MoneyManager.Model.Deposits;
using MoneyManager.Application.DTO.Deposits;

namespace MoneyManager.Application.Interfaces.Deposits
{
    public interface IDepositService
    {
        IEnumerable<ServerDepositDTO> GetAll(int monthsFrom, int monthsTo, bool onlyActive);
        
        Task<Guid> Add(ClientDepositDTO deposit);

        Task Update(ClientDepositDTO modifiedDeposit);

        Task Delete(Guid id);

        DepositMonthSummaryDTO GetSummary(int monthsFrom, int monthsTo, bool onlyActive);

        Task<DepositsRangeDTO> GetDepositsRange();
    }
}
