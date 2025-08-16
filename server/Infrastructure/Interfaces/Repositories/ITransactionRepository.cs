using MoneyManager.Infrastructure.Entities.Transactions;
using MoneyManager.Infrastructure.Interfaces.Database;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MoneyManager.Infrastructure.Interfaces.Repositories
{
    public interface ITransactionRepository: IRepository<Transaction>
    {
        IEnumerable<Transaction> GetAllFull(int momth, int year);

        Task<IEnumerable<string>> GetTypes();
    }
}