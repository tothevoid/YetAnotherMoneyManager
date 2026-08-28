using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Audex.Infrastructure.Entities.Deposits;
using Audex.Infrastructure.Interfaces.Database;

namespace Audex.Infrastructure.Interfaces.Repositories
{
    public interface IDepositRepository: IRepository<Deposit>
    {
        IEnumerable<Deposit> GetAllFull(Expression<Func<Deposit, bool>> predicate);
    }
}
