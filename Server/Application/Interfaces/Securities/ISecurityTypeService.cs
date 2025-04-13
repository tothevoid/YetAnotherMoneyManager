using MoneyManager.Application.DTO.Securities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MoneyManager.Application.Interfaces.Securities
{
    public interface ISecurityTypeService
    {
        Task<IEnumerable<SecurityTypeDTO>> GetAll();
        Task<Guid> Add(SecurityTypeDTO securityType);
        Task Update(SecurityTypeDTO securityType);
        Task Delete(Guid id);
    }
}
