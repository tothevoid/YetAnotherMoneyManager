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
    public interface IBrokerService
    {
        Task<IEnumerable<BrokerDto>> GetAll();
        Task<Guid> Add(BrokerDto security);
        Task Update(BrokerDto security);
        Task Delete(Guid id);
    }
}
