using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using MoneyManager.Shared.Entities;
using MoneyManager.Infrastructure.Interfaces.Database;
using MoneyManager.Infrastructure.Migrations;
using MoneyManager.Infrastructure.Queries;

namespace MoneyManager.Infrastructure.Database
{
    public class Repository<TEntity>: IRepository<TEntity>
        where TEntity: BaseEntity
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<TEntity> _entities;

        public Repository(ApplicationDbContext context)
        {
            _context = context;
            _entities = context.Set<TEntity>();
        }

        public async Task Add(TEntity entity)
        {
            await _entities.AddAsync(entity);
        }

        public async Task<TEntity> GetById(Guid id,
            Func<IQueryable<TEntity>, IQueryable<TEntity>> include = null,
            bool disableTracking = true)
        {
            IQueryable<TEntity> query =
                disableTracking ? _entities.AsQueryable().AsNoTracking() : _entities.AsQueryable();

            if (include != null)
            {
                query = include(query);
            }

            return await query.Where(entity => entity.Id == id).FirstOrDefaultAsync();
        }

        public async Task<TEntity> Find(Expression<Func<TEntity, bool>> predicate)
        {
            IQueryable<TEntity> query = _entities.AsNoTracking();
            return await query.Where(predicate).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<TEntity>> GetAll(Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IQueryable<TEntity>> include = null,
            bool disableTracking = true)
        {
            IQueryable<TEntity> query =
                disableTracking ? _entities.AsQueryable().AsNoTracking() : _entities.AsQueryable();

            if (include != null)
            {
                query = include(query);
            }

            if (filter != null)
            {
                query = query.Where(filter);
            }

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<TEntity>> GetAll(ComplexQuery<TEntity> complexQuery)
        {
            IQueryable<TEntity> query =
                complexQuery.TrackingDisabled ? _entities.AsQueryable().AsNoTracking() : _entities.AsQueryable();

            if (complexQuery.Filter != null)
            {
                query = query.Where(complexQuery.Filter);
            }

            while (complexQuery.OrderByExpressions.Count > 0)
            {
                var orderBy = complexQuery.OrderByExpressions.Dequeue();
                query = orderBy.IsDescending ? 
                    query.OrderByDescending(orderBy.Expression): 
                    query.OrderBy(orderBy.Expression);
            }

            if (complexQuery.RecordsOffset > 0)
            {
                query = query.Skip(complexQuery.RecordsOffset);
            }

            if (complexQuery.RecordsLimit > 0)
            {
                query = query.Take(complexQuery.RecordsLimit);
            }

            if (complexQuery.Joins != null)
            {
                query = complexQuery.Joins(query);
            }

            return await query.ToListAsync();
        }

        public async Task<int> GetCount(Expression<Func<TEntity, bool>> filter = null)
        {
            IQueryable<TEntity> query = _entities.AsQueryable().AsNoTracking();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            return await query.CountAsync();
        }

        public void Update(TEntity entity)
        {
            _entities.Update(entity);
        }

        public async Task Delete(Guid id)
        {
            IQueryable<TEntity> query = _entities.AsNoTracking();
            var entity = await query.FirstOrDefaultAsync(entity => entity.Id == id);

            if (entity == null)
            {
                return;
            }

            _entities.Remove(entity);
        }

        public async Task<TEntity> GetMin(Expression<Func<TEntity, object>> sortField)
        {
            IQueryable<TEntity> query = _entities.AsNoTracking();
            return await query.OrderBy(sortField).Take(1).FirstOrDefaultAsync(); ;
        }

        public async Task<TEntity> GetMax(Expression<Func<TEntity, object>> sortField)
        {
            IQueryable<TEntity> query = _entities.AsNoTracking();
            return await query.OrderByDescending(sortField).Take(1).FirstOrDefaultAsync(); ;
        }

        public async Task<decimal> GetSum(Expression<Func<TEntity, decimal>> projection, 
            Expression<Func<TEntity, bool>> filter = null)
        {
            IQueryable<TEntity> query = _entities.AsNoTracking();
            if (filter != null)
            {
                query = query.Where(filter);
            }

            return await query.SumAsync(projection);
        }

        public async Task SaveChanges() => await _context.SaveChangesAsync();

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}