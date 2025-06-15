using Microsoft.AspNetCore.Http;
using MoneyManager.Application.DTO.Securities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MoneyManager.Application.Interfaces.Securities
{
    public interface ISecurityService
    {
        Task<IEnumerable<SecurityDTO>> GetAll();
        Task<SecurityDTO> GetById(Guid id);

        Task<SecurityStatsDto> GetStats(Guid securityId);
        Task<IEnumerable<SecurityHistoryValueDto>> GetTickerHistory(string ticker);
        Task<Guid> Add(SecurityDTO security, IFormFile securityIcon);
        Task Update(SecurityDTO security, IFormFile securityIcon);
        Task<string> GetIconUrl(string iconKey);
        Task Delete(Guid id);
    }
}
