using MoneyManager.Application.DTO.Securities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MoneyManager.Application.Interfaces.Securities
{
    public interface ISecurityService
    {
        IEnumerable<SecurityDto> GetAll(bool onlyActive);
        Task<Guid> Add(SecurityDto security);
        Task Update(SecurityDto security);
        Task Delete(Guid id);
    }
}
