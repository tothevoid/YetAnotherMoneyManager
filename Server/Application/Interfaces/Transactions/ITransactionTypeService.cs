using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using MoneyManager.Application.DTO.Transactions;

namespace MoneyManager.Application.Interfaces.Transactions
{
    public interface ITransactionTypeService
    {
        Task<IEnumerable<TransactionTypeDTO>> GetAll();

        Task<TransactionTypeDTO> Add(TransactionTypeDTO transactionTypeDto);

        Task Update(TransactionTypeDTO transactionTypeDto);

        Task Delete(Guid id);
    }
}