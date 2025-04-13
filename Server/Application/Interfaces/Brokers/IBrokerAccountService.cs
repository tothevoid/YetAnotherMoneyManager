using MoneyManager.Application.DTO.Brokers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MoneyManager.Application.Interfaces.Brokers
{
    public interface IBrokerAccountService
    {
        Task<IEnumerable<BrokerAccountDto>> GetAll();
        Task<Guid> Add(BrokerAccountDto security);
        Task Update(BrokerAccountDto security);
        Task Delete(Guid id);
    }
}
