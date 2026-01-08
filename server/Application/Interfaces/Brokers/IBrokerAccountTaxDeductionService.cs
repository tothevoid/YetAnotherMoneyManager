using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MoneyManager.Application.DTO.Brokers;

namespace MoneyManager.Application.Interfaces.Brokers
{
    public interface IBrokerAccountTaxDeductionService
    {
        Task<IEnumerable<BrokerAccountTaxDeductionDto>> GetAll(Guid? brokerAccountId);
        Task<decimal> GetAmountByBrokerAccount(Guid brokerAccountId);
        Task<Guid> Add(BrokerAccountTaxDeductionDto dto);
        Task Update(BrokerAccountTaxDeductionDto dto);
        Task Delete(Guid id);
    }
}