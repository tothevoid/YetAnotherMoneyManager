using MoneyManager.Application.DTO.Currencies;
using MoneyManager.Application.DTO.Crypto;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MoneyManager.Application.Interfaces.Crypto
{
    public interface ICryptoAccountCryptocurrencyService
    {
        
        Task<IEnumerable<CryptoAccountCryptocurrencyDto>> GetByCryptoAccount(Guid cryptoAccountId);
        Task<IEnumerable<CryptoAccountCryptocurrencyDto>> GetAll();
        Task<Guid> Add(CryptoAccountCryptocurrencyDto cryptoAccountCryptocurrency);
        Task Update(CryptoAccountCryptocurrencyDto cryptoAccountCryptocurrency);
        Task Delete(Guid id);
    }
}