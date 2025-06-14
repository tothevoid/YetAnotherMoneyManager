﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using MoneyManager.Shared.Entities;

namespace MoneyManager.Infrastructure.Queries
{
    public class ComplexQuery<TEntity>
        where TEntity: BaseEntity
    {
        public Expression<Func<TEntity, bool>> Filter { get; set; }

        public Func<IQueryable<TEntity>, IQueryable<TEntity>> Joins { get; set; }

        public Queue<OrderByConfig<TEntity>> OrderByExpressions { get; set; } = new Queue<OrderByConfig<TEntity>>();

        public int RecordsLimit { get; set; } = -1;

        public int RecordsOffset { get; set; } = -1;

        public bool TrackingDisabled { get; set; }
    }

    public class OrderByConfig<TEntity>
    {
        public bool IsDescending { get; set; }

        public Expression<Func<TEntity, object>> Expression { get; set; }
    }
}
