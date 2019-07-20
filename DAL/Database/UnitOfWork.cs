using System;
using System.Collections.Generic;
using MoneyManager.Common;
using MoneyManager.DAL.Interfaces;

namespace MoneyManager.DAL.Database
{
    public class UnitOfWork : IDisposable, IUnitOfWork
    {
        private readonly IMongoContext _context;
        private Dictionary<string, object> repositories;

        public UnitOfWork(IMongoContext context)
        {
            _context = context;
        }

        public IRepository<T> CreateRepository<T>()
            where T : BaseEntity
        {
            if (repositories == null)
                repositories = new Dictionary<string, object>();

            var type = typeof(T).Name;

            if (!repositories.ContainsKey(type))
            {
                var repositoryType = typeof(Repository<>);
                var repositoryInstance = Activator.CreateInstance(repositoryType.MakeGenericType(typeof(T)), _context);
                repositories.Add(type, repositoryInstance);
            }
            return (Repository<T>)repositories[type];
        }

        public void Commit()
        {
            _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}