using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MoneyManager.Application.DTO.Accounts;

namespace MoneyManager.Application.Interfaces.Accounts
{
    public interface IAccountService
    {
        Task<IEnumerable<AccountDTO>> GetAll(bool onlyActive);
        Task<IEnumerable<AccountDTO>> GetAllByTypes(Guid[] typesIds, bool onlyActive);
        Task<AccountDTO> GetById(Guid id, bool disableTracking = true);
        Task<Guid> Add(AccountDTO transaction);
        Task Update(AccountDTO accountDTO);
        Task Delete(Guid id);
        Task Transfer(AccountTransferDTO transferDto);
        Task<AccountCurrencySummaryDTO[]> GetSummary();
    }
}