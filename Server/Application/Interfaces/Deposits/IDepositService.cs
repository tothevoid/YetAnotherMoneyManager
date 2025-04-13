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
        IEnumerable<ServerDepositDto> GetAll(int monthsFrom, int monthsTo, bool onlyActive);
        
        Task<Guid> Add(ClientDepositDto deposit);

        Task Update(ClientDepositDto modifiedDeposit);

        Task Delete(Guid id);

        DepositMonthSummaryDTO GetSummary(int monthsFrom, int monthsTo, bool onlyActive);

        Task<DepositsRangeDto> GetDepositsRange();
    }
}
