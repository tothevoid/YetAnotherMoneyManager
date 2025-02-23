using Microsoft.AspNetCore.Http;
using MoneyManager.BLL.DTO;
using MoneyManager.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MoneyManager.BLL.Services.Entities
{
    public interface ITransactionTypeService
    {
        //Task<TransactionTypeDTO> Add(string name, string extension, IFormFile formFile);

        //Task Update(Guid id, string name, string extension, IFormFile formFile);

        //Task Delete(Guid id);

        //Task<IEnumerable<TransactionTypeDTO>> GetAll();

        Task<IEnumerable<string>> GetAll();
    }
}