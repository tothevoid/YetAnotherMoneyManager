using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MoneyManager.Application.DTO.Securities;

namespace MoneyManager.Application.Interfaces.Securities
{
    public interface IDividendService
    {
        Task<IEnumerable<DividendDto>> GetAll(Guid securityId);

        Task<IEnumerable<DividendDto>> GetAvailable(Guid brokerAccountId);

        Task Update(DividendDto securityTypeDto);

        Task<Guid> Add(DividendDto securityDto);

        Task Delete(Guid id);
    }
}
