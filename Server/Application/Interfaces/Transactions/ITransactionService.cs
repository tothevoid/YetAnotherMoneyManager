using MoneyManager.Application.DTO.Transactions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MoneyManager.Application.Interfaces.Transactions
{
    public interface ITransactionsService
    {
        Task<TransactionDTO> GetById(Guid id);
        Task<IEnumerable<TransactionDTO>> GetAll(int month, int year, bool showSystem);
        Task<Guid> Add(TransactionDTO transaction);
        Task Delete(Guid id);
        Task<List<UpdateAccountDTO>> Update(TransactionDTO updateAccountModel);
    }
}