using MoneyManager.Application.DTO.Securities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MoneyManager.Application.DTO.Transactions;

namespace MoneyManager.Application.Interfaces.Transactions
{
    public interface ICurrencyTransactionService
    {
        Task<IEnumerable<CurrencyTransactionDto>> GetAll();
        Task<Guid> Add(CurrencyTransactionDto currencyTransactionDto);
        Task Update(CurrencyTransactionDto currencyTransactionDto);
        Task Delete(Guid id);
    }
}
