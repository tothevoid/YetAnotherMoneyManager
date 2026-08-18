using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using MoneyManager.Application.DTO.FileStorage;
using MoneyManager.Application.DTO.Transactions;

namespace MoneyManager.Application.Interfaces.Transactions
{
    public interface ITransactionTypeService
    {
        Task<IEnumerable<TransactionTypeDto>> GetAllAsync(bool onlyActive);

        Task<TransactionTypeDto> AddAsync(TransactionTypeDto transactionTypeDto, IFormFile transactionTypeIcon);

        Task<TransactionTypeDto> UpdateAsync(TransactionTypeDto transactionTypeDto, IFormFile transactionTypeIcon);

        Task DeleteAsync(Guid id);

        Task<FileStreamDto> GetIconStreamAsync(string iconKey);

        Task<string> GetIconUrlAsync(string iconKey);
    }
}