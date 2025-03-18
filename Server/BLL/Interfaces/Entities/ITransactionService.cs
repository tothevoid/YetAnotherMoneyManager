using MoneyManager.BLL.DTO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MoneyManager.BLL.Services.Entities
{
    public interface ITransactionsService
    {
        Task<IEnumerable<TransactionDTO>> GetAll(int month, int year);
        Task<Guid> Add(TransactionDTO transaction);
        Task Delete(Guid id);
        Task<List<UpdateAccountDTO>> Update(TransactionDTO updateAccountModel);
    }
}