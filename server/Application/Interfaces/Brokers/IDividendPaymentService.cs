using MoneyManager.Application.DTO.Brokers;
using MoneyManager.Application.DTO.Common;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MoneyManager.Application.Interfaces.Brokers
{
    public interface IDividendPaymentService
    {
        Task<IEnumerable<DividendPaymentDto>> GetAll(Guid brokerAccountId, int pageIndex, int recordsQuantity);

        Task<PaginationConfigDto> GetPagination(Guid brokerAccountId);

        Task<decimal> GetEarnings();

        Task<decimal> GetEarningsByBrokerAccount(Guid brokerAccountId);

        Task<Guid> Add(DividendPaymentDto dividendPaymentDto);

        Task Update(DividendPaymentDto dividendPaymentDto);

        Task Delete(Guid id);
    }
}
