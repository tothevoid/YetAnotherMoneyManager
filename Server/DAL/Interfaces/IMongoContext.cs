using MongoDB.Driver;
using System;
using System.Threading.Tasks;

namespace MoneyManager.DAL.Interfaces
{
    public interface IMongoContext : IDisposable
    {
        Task AddCommand(Func<Task> func);
        int SaveChanges();
        IMongoCollection<T> GetCollection<T>(string name);
    }
}
