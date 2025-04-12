using MoneyManager.BLL.DTO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MoneyManager.WEB.Model;
using BLL.DTO;

namespace MoneyManager.BLL.Services.Entities
{
    public interface IAccountService
    {
        IEnumerable<AccountDTO> GetAll(bool onlyActive);
        Task<Guid> Add(AccountDTO transaction);
        Task Update(AccountDTO accountDTO);
        Task Delete(Guid id);
        Task Transfer(AccountTransferDto transferDto);
        Task<AccountCurrencySummaryDto[]> GetSummary();
    }
}