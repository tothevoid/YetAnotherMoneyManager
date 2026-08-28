using MoneyManager.Application.DTO.Crypto;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MoneyManager.Application.Interfaces.Crypto
{
    public interface ICryptoAccountService
    {
        Task<CryptoAccountDto> GetByIdAsync(Guid id);
        Task<IEnumerable<CryptoAccountDto>> GetAllAsync();
        Task<Guid> AddAsync(CryptoAccountDto cryptoAccount);
        Task UpdateAsync(CryptoAccountDto cryptoAccount);
        Task DeleteAsync(Guid id);
    }
}