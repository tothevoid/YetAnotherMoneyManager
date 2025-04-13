using MoneyManager.Application.DTO.Brokers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MoneyManager.Application.Interfaces.Brokers
{
    public interface IBrokerService
    {
        Task<IEnumerable<BrokerDTO>> GetAll();
        Task<Guid> Add(BrokerDTO security);
        Task Update(BrokerDTO security);
        Task Delete(Guid id);
    }
}
