using MoneyManager.Application.DTO.Brokers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MoneyManager.Application.Interfaces.Brokers
{
    public interface IBrokerService
    {
        Task<IEnumerable<BrokerDto>> GetAllAsync();
        Task<Guid> AddAsync(BrokerDto security);
        Task UpdateAsync(BrokerDto security);
        Task DeleteAsync(Guid id);
    }
}
