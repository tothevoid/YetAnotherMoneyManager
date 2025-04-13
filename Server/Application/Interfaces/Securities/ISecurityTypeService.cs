using MoneyManager.Application.DTO.Securities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MoneyManager.Application.Interfaces.Securities
{
    public interface ISecurityTypeService
    {
        Task<IEnumerable<SecurityTypeDto>> GetAll();
        Task<Guid> Add(SecurityTypeDto securityType);
        Task Update(SecurityTypeDto securityType);
        Task Delete(Guid id);
    }
}
