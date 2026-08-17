using MoneyManager.Application.DTO.Common;
using MoneyManager.Application.DTO.Securities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MoneyManager.Application.Interfaces.Securities
{
    public interface IDividendService
    {
        Task<IEnumerable<DividendDto>> GetAllAsync(Guid securityId, int pageIndex, int recordsQuantity);

        Task<IEnumerable<DividendDto>> GetAvailableAsync(Guid brokerAccountId);

        Task<PaginationConfigDto> GetPaginationAsync(Guid securityId);

        Task UpdateAsync(DividendDto securityTypeDto);

        Task<Guid> AddAsync(DividendDto securityDto);

        Task DeleteAsync(Guid id);
    }
}
