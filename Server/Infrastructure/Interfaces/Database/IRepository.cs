using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MoneyManager.Infrastructure.Queries;
using MoneyManager.Shared.Entities;

namespace MoneyManager.Infrastructure.Interfaces.Database
{
    public interface IRepository<TEntity>: IDisposable
        where TEntity: BaseEntity
    {
        Task Add(TEntity entity);

        Task<TEntity> GetById(Guid id,
            Func<IQueryable<TEntity>, IQueryable<TEntity>> include = null,
            bool disableTracking = true);

        Task<IEnumerable<TEntity>> GetAll(Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IQueryable<TEntity>> include = null,
            bool disableTracking = true);
        Task<IEnumerable<TEntity>> GetAll(ComplexQuery<TEntity> complexQuery);

        Task<int> GetCount(Expression<Func<TEntity, bool>> filter = null);

        Task<TEntity> Find(Expression<Func<TEntity, bool>> predicate);

        void Update(TEntity entity);

        Task Delete(Guid id);

        Task<TEntity> GetMin(Expression<Func<TEntity, object>> sortField);

        Task<TEntity> GetMax(Expression<Func<TEntity, object>> sortField);

        Task<decimal> GetSum(Expression<Func<TEntity, decimal>> projection,
            Expression<Func<TEntity, bool>> filter = null);
    }
}