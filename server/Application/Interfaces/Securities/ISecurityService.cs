using Microsoft.AspNetCore.Http;
using MoneyManager.Application.DTO.Securities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MoneyManager.Application.Interfaces.Securities
{
    public interface ISecurityService
    {
        Task<IEnumerable<SecurityDTO>> GetAll(bool disableTracking = true);
        Task<SecurityDTO> FindByTicker(string ticker);

        Task<IEnumerable<SecurityDTO>> FindByTickers(IEnumerable<string> tickers);

        Task<SecurityDTO> GetById(Guid id, bool loadHierarchy = true, bool disableTracking = true);
        Task<SecurityStatsDto> GetStats(Guid securityId);
        Task<IEnumerable<SecurityHistoryValueDto>> GetTickerHistory(string ticker);
        Task<SecurityDTO> Add(SecurityDTO security, IFormFile securityIcon);
        Task<SecurityDTO> Update(SecurityDTO security, IFormFile securityIcon);
        Task<string> GetIconUrl(string iconKey);
        Task Delete(Guid id);
    }
}
