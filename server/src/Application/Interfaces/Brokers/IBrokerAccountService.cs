using Audex.Application.DTO.Brokers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Audex.Application.Interfaces.Brokers
{
    public interface IBrokerAccountService
    {
        Task<IEnumerable<BrokerAccountDto>> GetAllAsync();

        Task<BrokerAccountDto> GetByIdAsync(Guid id);

        Task<decimal> GetTotalSoldAmountByBrokerAccountAsync(Guid brokerAccountId);

        Task<Guid> AddAsync(BrokerAccountDto brokerAccount);

        Task UpdateAsync(BrokerAccountDto brokerAccount);

        Task DeleteAsync(Guid id);
    }
}
