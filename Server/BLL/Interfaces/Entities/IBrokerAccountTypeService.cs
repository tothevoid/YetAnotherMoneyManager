using BLL.DTO;
using MoneyManager.BLL.DTO;
using MoneyManager.WEB.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyManager.BLL.Interfaces.Entities
{
    public interface IBrokerAccountTypeService
    {
        Task<IEnumerable<BrokerAccountTypeDto>> GetAll();
        Task<Guid> Add(BrokerAccountTypeDto security);
        Task Update(BrokerAccountTypeDto security);
        Task Delete(Guid id);
    }
}
