using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MoneyManager.Application.DTO.Brokers;

namespace MoneyManager.Application.Interfaces.Brokers
{
    public interface IBrokerAccountTaxDeductionService
    {
        Task<IEnumerable<BrokerAccountTaxDeductionDto>> GetAllAsync(Guid? brokerAccountId);
        Task<decimal> GetSumTillSpecificDateAsync(DateOnly date, Guid? brokerAccountId);
        Task<decimal> GetAmountByBrokerAccountAsync(Guid brokerAccountId);
        Task<Guid> AddAsync(BrokerAccountTaxDeductionDto dto);
        Task UpdateAsync(BrokerAccountTaxDeductionDto dto);
        Task DeleteAsync(Guid id);
    }
}