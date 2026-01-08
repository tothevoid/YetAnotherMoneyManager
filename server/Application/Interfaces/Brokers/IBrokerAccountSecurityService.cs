using MoneyManager.Application.DTO.Brokers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MoneyManager.Application.Interfaces.Brokers
{
    public interface IBrokerAccountSecurityService
    {
        Task<IEnumerable<BrokerAccountSecurityDTO>> GetAll(bool unionSecurities = false);
        
        Task<IEnumerable<BrokerAccountSecurityDTO>> GetByBrokerAccount(Guid brokerAccountId);

        Task PullQuotations();

        Task PullQuotationsByBrokerAccount(Guid brokerAccountId);
        
        Task<Guid> Add(BrokerAccountSecurityDTO security);
        
        Task Update(BrokerAccountSecurityDTO security);
        
        Task Delete(Guid id);

        Task<decimal> GetInitialSecuritiesValue(Guid brokerAccountId);

        Task<decimal> GetActualSecuritiesValue(Guid brokerAccountId);
    }
}
