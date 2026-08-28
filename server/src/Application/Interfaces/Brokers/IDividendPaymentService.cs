using MoneyManager.Application.DTO.Brokers;
using MoneyManager.Application.DTO.Common;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MoneyManager.Application.Interfaces.Brokers
{
    public interface IDividendPaymentService
    {
        Task<IEnumerable<DividendPaymentDto>> GetAllAsync(Guid? brokerAccountId, int pageIndex, int recordsQuantity);

        Task<decimal> GetSumTillSpecificDateAsync(DateOnly date, Guid? brokerAccountId);

        Task<PaginationConfigDto> GetPaginationAsync();

        Task<PaginationConfigDto> GetPaginationByBrokerAccountAsync(Guid brokerAccountId);

        Task<decimal> GetEarningsAsync();

        Task<decimal> GetEarningsByBrokerAccountAsync(Guid brokerAccountId);

        Task<Guid> AddAsync(DividendPaymentDto dividendPaymentDto);

        Task UpdateAsync(DividendPaymentDto dividendPaymentDto);

        Task DeleteAsync(Guid id);
    }
}
