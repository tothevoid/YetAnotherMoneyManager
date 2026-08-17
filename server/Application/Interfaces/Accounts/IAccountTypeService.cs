using MoneyManager.Application.DTO.Accounts;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MoneyManager.Application.Interfaces.Accounts
{
    public interface IAccountTypeService
    {
        Task<IEnumerable<AccountTypeDto>> GetAllAsync();
        Task<Guid> AddAsync(AccountTypeDto accountType);
        Task UpdateAsync(AccountTypeDto accountType);
        Task DeleteAsync(Guid id);
    }
}
