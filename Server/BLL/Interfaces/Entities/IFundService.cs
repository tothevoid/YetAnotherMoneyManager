using MoneyManager.BLL.DTO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MoneyManager.BLL.Services.Entities
{
    public interface IFundService
    {
        Task<IEnumerable<FundDTO>> GetAll();
        Task<Guid> Add(FundDTO transaction);
        Task Update(FundDTO fundDTO);
        Task Delete(Guid id);
    }
}