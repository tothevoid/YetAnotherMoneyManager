using System.Collections.Generic;
using System.Threading.Tasks;

namespace MoneyManager.Application.Interfaces.Transactions
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