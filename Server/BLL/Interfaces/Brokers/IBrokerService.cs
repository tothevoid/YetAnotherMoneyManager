using MoneyManager.Application.DTO.Brokers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MoneyManager.Application.Interfaces.Brokers
{
    public interface IBrokerService
    {
        Task<IEnumerable<BrokerDto>> GetAll();
        Task<Guid> Add(BrokerDto security);
        Task Update(BrokerDto security);
        Task Delete(Guid id);
    }
}
