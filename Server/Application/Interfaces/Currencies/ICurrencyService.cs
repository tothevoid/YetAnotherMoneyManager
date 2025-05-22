using MoneyManager.Application.DTO.Currencies;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MoneyManager.Application.Interfaces.Currencies
{
    public interface ICurrencyService
    {
        Task SyncRates(CurrencyDTO mainCurrency);
        Task<IEnumerable<CurrencyDTO>> GetAll();
        Task<Guid> Add(CurrencyDTO currency);
        Task Update(CurrencyDTO currency);
        Task Delete(Guid id);
    }
}