using MoneyManager.Application.DTO.Accounts;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MoneyManager.Application.Interfaces.Accounts
{
    public interface IAccountTypeService
    {
        Task<IEnumerable<AccountTypeDTO>> GetAll();
        Task<Guid> Add(AccountTypeDTO accountType);
        Task Update(AccountTypeDTO accountType);
        Task Delete(Guid id);
    }
}
