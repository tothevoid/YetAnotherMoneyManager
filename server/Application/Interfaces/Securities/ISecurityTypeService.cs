using MoneyManager.Application.DTO.Securities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MoneyManager.Application.Interfaces.Securities
{
    public interface ISecurityTypeService
    {
        Task<IEnumerable<SecurityTypeDto>> GetAllAsync();
        Task<Guid> AddAsync(SecurityTypeDto securityType);
        Task UpdateAsync(SecurityTypeDto securityType);
        Task DeleteAsync(Guid id);
    }
}
