using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MoneyManager.Application.DTO.Accounts;

namespace MoneyManager.Application.Interfaces.Accounts
{
    public interface IAccountService
    {
        IEnumerable<AccountDTO> GetAll(bool onlyActive);
        Task<Guid> Add(AccountDTO transaction);
        Task Update(AccountDTO accountDTO);
        Task Delete(Guid id);
        Task Transfer(AccountTransferDTO transferDto);
        Task<AccountCurrencySummaryDTO[]> GetSummary();
    }
}