using Audex.Infrastructure.Entities.Transactions;
using Audex.Infrastructure.Interfaces.Database;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Audex.Infrastructure.Interfaces.Repositories
{
    public interface ITransactionRepository: IRepository<Transaction>
    {
        IEnumerable<Transaction> GetAllFull(int momth, int year);

        Task<IEnumerable<string>> GetTypes();
    }
}