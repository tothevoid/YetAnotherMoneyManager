using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using MoneyManager.Shared.Entities;

namespace MoneyManager.Infrastructure.Queries
{
    public class ComplexQueryBuilder<TEntity>
        where TEntity: BaseEntity
    {
        private readonly ComplexQuery<TEntity> _complexQuery = new();

        public ComplexQueryBuilder<TEntity> AddFilter(Expression<Func<TEntity, bool>> filter)
        {
            _complexQuery.Filter = filter;
            return this;
        }

        public ComplexQueryBuilder<TEntity> AddJoins(Func<IQueryable<TEntity>, IQueryable<TEntity>> joins)
        {
            _complexQuery.Joins = joins;
            return this;
        }

        public ComplexQueryBuilder<TEntity> AddOrder(Expression<Func<TEntity, object>> orderBy,
            bool isDescending = false)
        {
            _complexQuery.OrderBy = orderBy;
            _complexQuery.IsDescending = isDescending;

            return this;
        }

        public ComplexQueryBuilder<TEntity> AddPagination(Expression<Func<TEntity, object>> orderBy,
            int recordsLimit,
            int recordsOffset,
            bool isDescending = false)
        {
            AddOrder(orderBy, isDescending);

            _complexQuery.RecordsLimit = recordsLimit;
            _complexQuery.RecordsOffset = recordsOffset;

            return this;
        }

        public ComplexQueryBuilder<TEntity> DisableTracking()
        {
            _complexQuery.TrackingDisabled = true;
            return this;
        }

        public ComplexQuery<TEntity> GetQuery()
        {
            return _complexQuery;
        }
    }
}
