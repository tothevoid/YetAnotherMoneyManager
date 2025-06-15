using MoneyManager.Application.DTO.Currencies;
using MoneyManager.Infrastructure.Entities.Crypto;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MoneyManager.Application.Interfaces.Crypto
{
    public interface ICryptocurrencyService
    {
        Task<IEnumerable<CryptocurrencyDto>> GetAll();
        Task<Guid> Add(CryptocurrencyDto cryptocurrency);
        Task Update(CryptocurrencyDto cryptocurrency);
        Task Delete(Guid id);
    }
}