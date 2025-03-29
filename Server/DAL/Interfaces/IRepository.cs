using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MoneyManager.DAL.Interfaces
{
    public interface IRepository<TEntity>: IDisposable
        where TEntity: class
    {
        Task Add(TEntity entity);

        Task<TEntity> GetById(Guid id);

        Task<IEnumerable<TEntity>> GetAll();

        Task<IEnumerable<TEntity>> GetAll(Expression<Func<TEntity, bool>> predicate);

        Task Update(TEntity entity);

        Task Delete(Guid id);

        Task Increment(Guid id, Expression<Func<TEntity, decimal>> field, decimal delta);
    }
}