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
    public interface IBrokerAccountSecurityService
    {
        Task<IEnumerable<BrokerAccountSecurityDto>> GetAll();
        Task<Guid> Add(BrokerAccountSecurityDto security);
        Task Update(BrokerAccountSecurityDto security);
        Task Delete(Guid id);
    }
}
