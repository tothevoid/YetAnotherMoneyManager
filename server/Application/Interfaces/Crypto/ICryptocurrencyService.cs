using MoneyManager.Application.DTO.Currencies;
using MoneyManager.Infrastructure.Entities.Crypto;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace MoneyManager.Application.Interfaces.Crypto
{
    public interface ICryptocurrencyService
    {
        Task<IEnumerable<CryptocurrencyDto>> GetAll();
        Task<CryptocurrencyDto> Add(CryptocurrencyDto cryptocurrency, IFormFile cryptocurrencyIcon);
        Task<CryptocurrencyDto> Update(CryptocurrencyDto cryptocurrency, IFormFile cryptocurrencyIcon);
        Task Delete(Guid id);
        Task<string> GetIconUrl(string iconKey);
    }
}