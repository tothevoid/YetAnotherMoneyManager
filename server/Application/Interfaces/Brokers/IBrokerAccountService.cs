using MoneyManager.Application.DTO.Brokers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MoneyManager.Application.Interfaces.Brokers
{
    public interface IBrokerAccountService
    {
        Task<IEnumerable<BrokerAccountDTO>> GetAll();
        Task<BrokerAccountDTO> GetById(Guid id, bool disableTracking = true);
        Task<Guid> Add(BrokerAccountDTO security);
        Task Update(BrokerAccountDTO security);
        Task Delete(Guid id);
    }
}
