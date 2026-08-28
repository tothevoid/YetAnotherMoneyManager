using MoneyManager.Application.DTO.Currencies;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MoneyManager.Application.Interfaces.Currencies
{
    public interface ICurrencyService
    {
        Task SyncRatesAsync(CurrencyDto mainCurrency);
        Task<IEnumerable<CurrencyDto>> GetAllAsync();
        Task<CurrencyDto> GetByIdAsync(Guid id);
        Task<Guid> AddAsync(CurrencyDto currency);
        Task UpdateAsync(CurrencyDto currency);
        Task DeleteAsync(Guid id);
    }
}