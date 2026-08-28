using Audex.Application.DTO.Brokers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Audex.Application.Interfaces.Brokers
{
    public interface IBrokerAccountTypeService
    {
        Task<IEnumerable<BrokerAccountTypeDto>> GetAllAsync();
        Task<Guid> AddAsync(BrokerAccountTypeDto security);
        Task UpdateAsync(BrokerAccountTypeDto security);
        Task DeleteAsync(Guid id);
    }
}
