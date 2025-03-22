using MoneyManager.BLL.DTO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MoneyManager.WEB.Model;

namespace MoneyManager.BLL.Services.Entities
{
    public interface IAccountService
    {
        Task<IEnumerable<AccountDTO>> GetAll();
        Task<Guid> Add(AccountDTO transaction);
        Task Update(AccountDTO accountDTO);
        Task Delete(Guid id);
        Task Transfer(AccountTransferDto transferDto);
    }
}