using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using MoneyManager.Infrastructure.Entities.Accounts;
using MoneyManager.Infrastructure.Interfaces.Database;

namespace MoneyManager.Infrastructure.Interfaces.Repositories
{
    public interface IAccountRepository : IRepository<Account>
    {
        IEnumerable<Account> GetAllFull(Expression<Func<Account, bool>> predicate = null);
    }
}
