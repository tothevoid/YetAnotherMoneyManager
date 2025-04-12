using MoneyManager.BLL.DTO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MoneyManager.BLL.Interfaces.Entities
{
    public interface IBrokerAccountService
    {
        Task<IEnumerable<BrokerAccountDto>> GetAll();
        Task<Guid> Add(BrokerAccountDto security);
        Task Update(BrokerAccountDto security);
        Task Delete(Guid id);
    }
}
