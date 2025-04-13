using MongoDB.Driver;
using System;
using System.Threading.Tasks;

namespace MoneyManager.Infrastructure.Interfaces.Database
{
    public interface IMongoContext : IDisposable
    {
        Task AddCommand(Func<Task> func);
        int SaveChanges();
        IMongoCollection<T> GetCollection<T>(string name);
    }
}
