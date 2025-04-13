using MoneyManager.Application.DTO.Brokers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MoneyManager.Application.Interfaces.Brokers
{
    public interface IBrokerAccountTypeService
    {
        Task<IEnumerable<BrokerAccountTypeDTO>> GetAll();
        Task<Guid> Add(BrokerAccountTypeDTO security);
        Task Update(BrokerAccountTypeDTO security);
        Task Delete(Guid id);
    }
}
