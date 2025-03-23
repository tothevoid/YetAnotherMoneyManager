using BLL.DTO;
using MoneyManager.BLL.DTO;
using MoneyManager.WEB.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyManager.BLL.Services.Entities
{
    public interface IAccountTypeService
    {
        Task<IEnumerable<AccountTypeDTO>> GetAll();
        Task<Guid> Add(AccountTypeDTO accountType);
        Task Update(AccountTypeDTO accountType);
        Task Delete(Guid id);
    }
}
