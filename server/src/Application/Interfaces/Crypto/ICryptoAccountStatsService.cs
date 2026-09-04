using Audex.Application.DTO.Crypto;
using System;
using System.Threading.Tasks;

namespace Audex.Application.Interfaces.Crypto
{
    public interface ICryptoAccountStatsService
    {
        Task<CryptoAccountStatsDto> GetStatsAsync();
        Task<CryptoAccountStatsDto> GetStatsByCryptoAccountAsync(Guid cryptoAccountId);
    }
}
