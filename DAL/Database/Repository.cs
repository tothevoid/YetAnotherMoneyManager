using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MoneyManager.DAL.Interfaces;
using MongoDB.Driver;
using System.Collections.Generic;
using MoneyManager.Common;

namespace MoneyManager.DAL.Database
{
    public class Repository<TEntity>: IDisposable, IRepository<TEntity>
        where TEntity: BaseEntity
    {
        protected readonly IMongoContext _context;
        protected readonly IMongoCollection<TEntity> DbSet;

        public Repository(IMongoContext context)
        {
            _context = context;
            DbSet = _context.GetCollection<TEntity>(typeof(TEntity).Name);
        }

        public async Task Add(TEntity entity)
        {
            await _context.AddCommand(async () => await DbSet.InsertOneAsync(entity));
        }

        public async Task<TEntity> GetById(Guid id)
        {
            var data = await DbSet.FindAsync(Builders<TEntity>.Filter.Eq("_id", id));
            return data.FirstOrDefault();
        }

        public async Task<IEnumerable<TEntity>> GetAll()
        {
            var entities = await DbSet.FindAsync(Builders<TEntity>.Filter.Empty);
            return entities.ToList();
        }

        public async Task<IEnumerable<TEntity>> GetAll(Expression<Func<TEntity, bool>> predicate)
        {
            var entities = await DbSet.FindAsync(Builders<TEntity>.Filter.Where(predicate));
            return entities.ToList();
        }

        public async Task Update(TEntity entity)
        {
            await _context.AddCommand(async () =>
            {
                await DbSet.ReplaceOneAsync(Builders<TEntity>.Filter.Eq("_id", entity.Id), entity);
            });
        }

        public async Task Delete(Guid id)
        {
            await _context.AddCommand(async () => await DbSet.DeleteOneAsync(Builders<TEntity>.Filter.Eq("_id", id)));
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}