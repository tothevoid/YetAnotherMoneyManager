using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MoneyManager.Application.DTO.Transactions;

namespace MoneyManager.Application.Interfaces.Transactions
{
    public interface ICurrencyTransactionService
    {
        Task<IEnumerable<CurrencyTransactionDto>> GetAllAsync();
        Task<Guid> AddAsync(CurrencyTransactionDto currencyTransactionDto);
        Task UpdateAsync(CurrencyTransactionDto currencyTransactionDto);
        Task DeleteAsync(Guid id);
        Task<CurrencyTransactionDto> GetByIdAsync(Guid id);
        Task<IEnumerable<CurrencyTransactionDto>> GetAllByAccountIdAsync(Guid accountId);
    }
}
