using MoneyManager.Application.DTO.Brokers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MoneyManager.Application.Interfaces.Brokers
{
    public interface IDividendPaymentService
    {
        Task<IEnumerable<DividendPaymentDto>> GetAll(Guid brokerAccountId);

        Task<Guid> Add(DividendPaymentDto dividendPaymentDto);

        Task Update(DividendPaymentDto dividendPaymentDto);

        Task Delete(Guid id);
    }
}
