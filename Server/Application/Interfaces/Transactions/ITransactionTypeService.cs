using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using MoneyManager.Application.DTO.Transactions;

namespace MoneyManager.Application.Interfaces.Transactions
{
    public interface ITransactionTypeService
    {
        Task<IEnumerable<TransactionTypeDTO>> GetAll(bool onlyActive);

        Task<TransactionTypeDTO> Add(TransactionTypeDTO transactionTypeDto, IFormFile? transactionTypeIcon);

        Task<TransactionTypeDTO> Update(TransactionTypeDTO transactionTypeDto, IFormFile? transactionTypeIcon);

        Task Delete(Guid id);

        Task<string> GetIconUrl(string iconKey);
    }
}