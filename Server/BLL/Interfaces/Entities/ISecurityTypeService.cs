using MoneyManager.BLL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyManager.BLL.Interfaces.Entities
{
    public interface ISecurityTypeService
    {
        Task<IEnumerable<SecurityTypeDto>> GetAll();
        Task<Guid> Add(SecurityTypeDto securityType);
        Task Update(SecurityTypeDto securityType);
        Task Delete(Guid id);
    }
}
