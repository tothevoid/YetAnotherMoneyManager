using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using MoneyManager.Infrastructure.Entities.Brokers;
using MoneyManager.Infrastructure.Interfaces.Database;

namespace MoneyManager.Infrastructure.Interfaces.Repositories
{
    public interface IBrokerAccountSecurityRepository: IRepository<BrokerAccountSecurity>
    {
        IEnumerable<BrokerAccountSecurity> GetAllFull(Expression<Func<BrokerAccountSecurity, bool>> predicate = null);
    }
}
