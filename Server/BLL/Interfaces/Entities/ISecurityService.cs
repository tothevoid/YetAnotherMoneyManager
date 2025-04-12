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
    public interface ISecurityService
    {
        IEnumerable<SecurityDto> GetAll(bool onlyActive);
        Task<Guid> Add(SecurityDto security);
        Task Update(SecurityDto security);
        Task Delete(Guid id);
    }
}
