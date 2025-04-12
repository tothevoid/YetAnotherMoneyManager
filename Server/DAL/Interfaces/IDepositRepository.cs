using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using MoneyManager.DAL.Entities;

namespace MoneyManager.DAL.Interfaces
{
    public interface IDepositRepository: IRepository<Deposit>
    {
        IEnumerable<Deposit> GetAllFull(Expression<Func<Deposit, bool>> predicate);
    }
}
