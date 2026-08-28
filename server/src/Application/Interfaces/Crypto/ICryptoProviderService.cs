using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Audex.Application.DTO.Crypto;

namespace Audex.Application.Interfaces.Crypto
{
    public interface ICryptoProviderService
    {
        Task<IEnumerable<CryptoProviderDto>> GetAllAsync();
        Task<Guid> AddAsync(CryptoProviderDto cryptoProvider);
        Task UpdateAsync(CryptoProviderDto cryptoProvider);
        Task DeleteAsync(Guid id);
    }
}