using MoneyManager.Application.DTO.Currencies;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MoneyManager.Infrastructure.Entities.Crypto;

namespace MoneyManager.Application.Interfaces.Crypto
{
    public interface ICryptoProviderService
    {
        Task<IEnumerable<CryptoProviderDto>> GetAll();
        Task<Guid> Add(CryptoProviderDto cryptoProvider);
        Task Update(CryptoProviderDto cryptoProvider);
        Task Delete(Guid id);
    }
}