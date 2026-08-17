using MoneyManager.Application.DTO.Transactions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MoneyManager.Application.Interfaces.Transactions
{
    public interface ITransactionsService
    {
        Task<TransactionDto> GetByIdAsync(Guid id);
        Task<IEnumerable<TransactionDto>> GetAllAsync(int month, int year, bool showSystem);
        Task<TransactionDto> AddAsync(TransactionDto transaction);
        Task DeleteAsync(Guid id);
        Task UpdateAsync(TransactionDto transactionToUpdate);
    }
}