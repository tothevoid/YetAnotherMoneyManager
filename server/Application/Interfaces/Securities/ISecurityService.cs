using Microsoft.AspNetCore.Http;
using MoneyManager.Application.DTO.FileStorage;
using MoneyManager.Application.DTO.Securities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MoneyManager.Application.Interfaces.Securities
{
    public interface ISecurityService
    {
        Task<IEnumerable<SecurityDto>> GetAllAsync(bool disableTracking = true);
        Task<SecurityDto> FindByTickerAsync(string ticker);

        Task<IEnumerable<SecurityDto>> FindByTickersAsync(IEnumerable<string> tickers);

        Task<SecurityDto> GetByIdAsync(Guid id, bool loadHierarchy = true, bool disableTracking = true);
        Task<SecurityStatsDto> GetStatsAsync(Guid securityId);
        Task<IEnumerable<SecurityHistoryValueDto>> GetTickerHistoryAsync(string ticker, SecurityHistoryPeriod period = SecurityHistoryPeriod.Day1);
        Task<SecurityDto> AddAsync(SecurityDto security, IFormFile securityIcon);
        Task<SecurityDto> UpdateAsync(SecurityDto security, IFormFile securityIcon);
        Task<FileStreamDto> GetIconStreamAsync(string iconKey);
        Task<string> GetIconUrlAsync(string iconKey);
        Task DeleteAsync(Guid id);
    }
}
