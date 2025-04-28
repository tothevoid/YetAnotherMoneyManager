using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using MoneyManager.Infrastructure.Database;
using MoneyManager.Infrastructure.Entities.Brokers;
using MoneyManager.Infrastructure.Entities.Securities;
using MoneyManager.Infrastructure.Interfaces.Database;
using MoneyManager.Infrastructure.Interfaces.Repositories;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace MoneyManager.Infrastructure.SpecificRepositories
{
    public class BrokerAccountSecurityRepository: Repository<BrokerAccountSecurity>, IBrokerAccountSecurityRepository
    {
        public BrokerAccountSecurityRepository(IMongoContext context) : base(context) { }

        public IEnumerable<BrokerAccountSecurity> GetAllFull(Expression<Func<BrokerAccountSecurity, bool>> predicate = null)
        {
            var brokerAccountCollection = _context.GetCollection<BrokerAccount>(nameof(BrokerAccount));
            var securityCollection = _context.GetCollection<Security>(nameof(Security));
            //TODO: fix AsEnumerable then mongo driver will support inner join

            var dbSetQuery = DbSet.AsQueryable();

            if (predicate != null)
            {
                dbSetQuery = dbSetQuery.Where(predicate);
            }

            var query = (from brokerAccountSecurity in dbSetQuery
                join brokerAccount in brokerAccountCollection.AsQueryable() on brokerAccountSecurity.BrokerAccountId equals brokerAccount.Id into brokerAccountJoin
                from joinedBrokerAccount in brokerAccountJoin.DefaultIfEmpty()
                join security in securityCollection.AsQueryable() on brokerAccountSecurity.SecurityId equals security.Id into securityJoin
                from joinedSecurity in securityJoin.DefaultIfEmpty()
                select new { brokerAccountSecurity, joinedBrokerAccount, joinedSecurity }).ToList();

            return query.Select(x => x.brokerAccountSecurity
                .AssignReferences(x.joinedBrokerAccount, x.joinedSecurity));
        }
    }
}
