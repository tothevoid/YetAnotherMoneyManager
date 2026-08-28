using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Audex.Infrastructure.Entities.Brokers;
using Audex.Infrastructure.Interfaces.Database;

namespace Audex.Infrastructure.Interfaces.Repositories
{
    public interface IBrokerAccountSecurityRepository: IRepository<BrokerAccountSecurity>
    {
        IEnumerable<BrokerAccountSecurity> GetAllFull(Expression<Func<BrokerAccountSecurity, bool>> predicate = null);
    }
}
