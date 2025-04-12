using BLL.DTO;
using MoneyManager.BLL.DTO;
using MoneyManager.Model.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MoneyManager.Model.Deposits;

namespace BLL.Interfaces.Entities
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
