using MoneyManager.Application.DTO.Currencies;
using MoneyManager.Infrastructure.Entities.Crypto;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MoneyManager.Application.Interfaces.Crypto
{
    public interface ICryptoAccountService
    {
        Task<IEnumerable<CryptoAccountDto>> GetAll();
        Task<Guid> Add(CryptoAccountDto cryptoAccount);
        Task Update(CryptoAccountDto cryptoAccount);
        Task Delete(Guid id);
    }
}