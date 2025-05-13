using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MoneyManager.Application.DTO.Securities;

namespace MoneyManager.Application.Interfaces.Securities
{
    public interface IDividendService
    {
        public Task<IEnumerable<DividendDto>> GetAll(Guid securityId);

        public Task Update(DividendDto securityTypeDto);

        public Task<Guid> Add(DividendDto securityDto);

        public Task Delete(Guid id);
    }
}
