using MoneyManager.Application.DTO.Securities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MoneyManager.Application.Interfaces.Securities
{
    public interface ISecurityService
    {
        Task<IEnumerable<SecurityDTO>> GetAll();
        Task<SecurityDTO> GetById(Guid id);
        Task<Guid> Add(SecurityDTO security);
        Task Update(SecurityDTO security);
        Task Delete(Guid id);
    }
}
