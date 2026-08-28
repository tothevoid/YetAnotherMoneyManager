using System.Threading.Tasks;
using Audex.Infrastructure.Interfaces.Repositories;
using Audex.Shared.Entities;

namespace Audex.Infrastructure.Interfaces.Database
{
    public interface IUnitOfWork
    {
        IRepository<T> CreateRepository<T>() 
            where T: class;

        Task CommitAsync();
    }
}