using MoneyManager.Application.DTO.Common;
using MoneyManager.Application.DTO.Securities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace MoneyManager.Application.Interfaces.Securities
{
    public interface IDividendService
    {
        Task<IEnumerable<DividendDto>> GetAll(Guid securityId, int pageIndex, int recordsQuantity);

        Task<IEnumerable<DividendDto>> GetAvailable(Guid brokerAccountId);

        Task<PaginationConfigDto> GetPagination(Guid securityId);

        Task Update(DividendDto securityTypeDto);

        Task<Guid> Add(DividendDto securityDto);

        Task Delete(Guid id);
    }
}
