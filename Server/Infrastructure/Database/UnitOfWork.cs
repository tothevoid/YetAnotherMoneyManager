using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MoneyManager.Infrastructure.Interfaces.Database;
using MoneyManager.Shared.Entities;

namespace MoneyManager.Infrastructure.Database
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private bool _disposed;
        private Dictionary<string, object> _repositories;

        public UnitOfWork(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public IRepository<T> CreateRepository<T>()
            where T : BaseEntity
        {
            _repositories ??= new Dictionary<string, object>();

            var type = typeof(T).Name;

            if (_repositories.ContainsKey(type)) return (Repository<T>)_repositories[type];

            var repositoryType = typeof(Repository<>);
            var repositoryInstance = Activator.CreateInstance(repositoryType.MakeGenericType(typeof(T)), 
                _applicationDbContext);
            _repositories.Add(type, repositoryInstance);
            return (Repository<T>)_repositories[type];
        }

        public async Task Commit()
        {
            await _applicationDbContext.SaveChangesAsync();
        }

        public void Dispose(bool disposing)
        {
            if (_disposed) return;

            if (disposing)
            {
                _applicationDbContext.Dispose();
            }
            _disposed = true;
        }
    }
}