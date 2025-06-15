using MoneyManager.Application.DTO.Currencies;
using MoneyManager.Infrastructure.Entities.Crypto;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MoneyManager.Application.Interfaces.Crypto
{
    public interface ICryptoAccountCryptocurrencyService
    {
        Task<IEnumerable<CryptoAccountCryptocurrencyDto>> GetAll();
        Task<Guid> Add(CryptoAccountCryptocurrencyDto cryptoAccountCryptocurrency);
        Task Update(CryptoAccountCryptocurrencyDto cryptoAccountCryptocurrency);
        Task Delete(Guid id);
    }
}