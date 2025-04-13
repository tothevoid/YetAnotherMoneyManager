using MoneyManager.Application.DTO.Brokers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MoneyManager.Application.Interfaces.Brokers
{
    public interface IBrokerAccountTypeService
    {
        Task<IEnumerable<BrokerAccountTypeDto>> GetAll();
        Task<Guid> Add(BrokerAccountTypeDto security);
        Task Update(BrokerAccountTypeDto security);
        Task Delete(Guid id);
    }
}
