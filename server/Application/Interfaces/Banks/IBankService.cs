using Microsoft.AspNetCore.Http;
using MoneyManager.Application.DTO.Banks;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MoneyManager.Application.Interfaces.Banks
{
    public interface IBankService
    {
        Task<IEnumerable<BankDto>> GetAllAsync();
        Task<BankDto> GetByIdAsync(Guid id);
        Task<BankDto> AddAsync(BankDto bankDto, IFormFile bankIcon);
        Task<BankDto> UpdateAsync(BankDto bankDto, IFormFile bankIcon);
        Task<bool> DeleteAsync(Guid id);
        Task<string> GetIconUrlAsync(string iconKey);
    }
}