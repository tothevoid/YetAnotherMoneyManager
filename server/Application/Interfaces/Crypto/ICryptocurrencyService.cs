using MoneyManager.Application.DTO.Crypto;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace MoneyManager.Application.Interfaces.Crypto
{
    public interface ICryptocurrencyService
    {
        Task<IEnumerable<CryptocurrencyDto>> GetAllAsync();
        Task<CryptocurrencyDto> AddAsync(CryptocurrencyDto cryptocurrency, IFormFile cryptocurrencyIcon);
        Task<CryptocurrencyDto> UpdateAsync(CryptocurrencyDto cryptocurrency, IFormFile cryptocurrencyIcon);
        Task DeleteAsync(Guid id);
        Task<string> GetIconUrlAsync(string iconKey);
    }
}