using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using MoneyManager.Infrastructure.Database;
using MoneyManager.Infrastructure.Entities.Accounts;
using MoneyManager.Infrastructure.Entities.Deposits;
using MoneyManager.Infrastructure.Interfaces.Database;
using MoneyManager.Infrastructure.Interfaces.Repositories;
using MongoDB.Driver;

namespace MoneyManager.Infrastructure.SpecificRepositories
{
    internal class DepositRepository : Repository<Deposit>, IDepositRepository
    {
        public DepositRepository(IMongoContext context) : base(context) { }

        public IEnumerable<Deposit> GetAllFull(Expression<Func<Deposit, bool>> predicate)
        {
            var accountCollection = _context.GetCollection<Account>(nameof(Account));
            //TODO: fix AsEnumerable then mongo driver will support inner join
            var query = (from deposit in DbSet.AsQueryable().Where(predicate)
                join account in accountCollection.AsQueryable() on deposit.AccountId equals account.Id into accountJoin
                from joinedAccount in accountJoin.DefaultIfEmpty()
                select new { deposit, joinedAccount }).ToList();

            return query.Select(x => x.deposit.AssignReferences(x.joinedAccount));
        }
    }
}