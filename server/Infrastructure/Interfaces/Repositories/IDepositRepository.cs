using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using MoneyManager.Infrastructure.Entities.Deposits;
using MoneyManager.Infrastructure.Interfaces.Database;

namespace MoneyManager.Infrastructure.Interfaces.Repositories
{
    public interface IDepositRepository: IRepository<Deposit>
    {
        IEnumerable<Deposit> GetAllFull(Expression<Func<Deposit, bool>> predicate);
    }
}
