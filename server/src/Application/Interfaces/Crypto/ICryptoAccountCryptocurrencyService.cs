using Audex.Application.DTO.Crypto;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Audex.Application.Interfaces.Crypto
{
    public interface ICryptoAccountCryptocurrencyService
    {
        Task<IEnumerable<CryptoAccountCryptocurrencyDto>> GetByCryptoAccountAsync(Guid cryptoAccountId);
        Task<IEnumerable<CryptoAccountCryptocurrencyDto>> GetAllAsync();
        Task<Guid> AddAsync(CryptoAccountCryptocurrencyDto cryptoAccountCryptocurrency);
        Task UpdateAsync(CryptoAccountCryptocurrencyDto cryptoAccountCryptocurrency);
        Task DeleteAsync(Guid id);
        Task<decimal> GetTotalBalanceAsync();
        Task<decimal> GetTotalBalanceByCryptoAccountAsync(Guid cryptoAccountId);
    }
}