using MoneyManager.Application.DTO.Brokers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MoneyManager.Application.Interfaces.Brokers
{
    public interface IBrokerAccountSecurityService
    {
        Task<IEnumerable<BrokerAccountSecurityDto>> GetAll();
        Task<Guid> Add(BrokerAccountSecurityDto security);
        Task Update(BrokerAccountSecurityDto security);
        Task Delete(Guid id);
    }
}
