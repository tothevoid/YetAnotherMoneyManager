using MoneyManager.BLL.DTO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MoneyManager.BLL.Services.Entities
{
    public interface ICurrencyService
    {
        Task<IEnumerable<CurrencyDTO>> GetAll();
        Task<Guid> Add(CurrencyDTO currency);
        Task Update(CurrencyDTO currency);
        Task Delete(Guid id);
    }
}