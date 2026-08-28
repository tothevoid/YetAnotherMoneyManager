using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Audex.Infrastructure.Entities.Accounts;
using Audex.Infrastructure.Interfaces.Database;

namespace Audex.Infrastructure.Interfaces.Repositories
{
    public interface IAccountRepository : IRepository<Account>
    {
        IEnumerable<Account> GetAllFull(Expression<Func<Account, bool>> predicate = null);
    }
}
