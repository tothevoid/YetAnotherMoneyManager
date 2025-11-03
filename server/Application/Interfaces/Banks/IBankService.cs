using Microsoft.AspNetCore.Http;
using MoneyManager.Application.DTO.Banks;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MoneyManager.Application.Interfaces.Banks
{
    public interface IBankService
    {
        Task<IEnumerable<BankDto>> GetAll();
        Task<BankDto> GetById(Guid id);
        Task<BankDto> Add(BankDto bankDto, IFormFile bankIcon);
        Task<BankDto> Update(BankDto bankDto, IFormFile bankIcon);
        Task<bool> Delete(Guid id);
        Task<string> GetIconUrl(string iconKey);
    }
}