using MoneyManager.Application.DTO.Brokers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MoneyManager.Application.Interfaces.Brokers
{
    public interface IBrokerAccountSecurityService
    {
        Task<IEnumerable<BrokerAccountSecurityDTO>> GetAll(Guid brokerAccountId, int recordsQuantity, int pageIndex);
        Task<IEnumerable<BrokerAccountSecurityDTO>> GetByBrokerAccount(Guid brokerAccountId);
        Task<BrokerAccountSecurityPaginationDto> GetPagination(Guid brokerAccountId);
        Task<Guid> Add(BrokerAccountSecurityDTO security);
        Task Update(BrokerAccountSecurityDTO security);
        Task Delete(Guid id);
    }
}
