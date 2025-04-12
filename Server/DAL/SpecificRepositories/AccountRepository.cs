using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using MoneyManager.Infrastructure.Database;
using MoneyManager.Infrastructure.Entities.Accounts;
using MoneyManager.Infrastructure.Entities.Currencies;
using MoneyManager.Infrastructure.Interfaces.Database;
using MoneyManager.Infrastructure.Interfaces.Repositories;
using MongoDB.Driver;

namespace MoneyManager.Infrastructure.SpecificRepositories
{
    internal class AccountRepository: Repository<Account>, IAccountRepository
    {
        public AccountRepository(IMongoContext context) : base(context) { }

        public IEnumerable<Account> GetAllFull(Expression<Func<Account, bool>> predicate = null)
        {
            var currencyCollection = _context.GetCollection<Currency>(nameof(Currency));
            var accountTypeCollection = _context.GetCollection<AccountType>(nameof(AccountType));
            //TODO: fix AsEnumerable then mongo driver will support inner join

            var dbSetQuery = DbSet.AsQueryable();

            if (predicate != null)
            {
                dbSetQuery = dbSetQuery.Where(predicate);
            }

            var query = (from account in dbSetQuery
                join currency in currencyCollection.AsQueryable() on account.CurrencyId equals currency.Id into currencyJoin
                from joinedCurrency in currencyJoin.DefaultIfEmpty()
                join accountType in accountTypeCollection.AsQueryable() on account.AccountTypeId equals accountType.Id into accountTypeJoin
                from joinedAccountType in accountTypeJoin.DefaultIfEmpty()
                select new { account, joinedCurrency, joinedAccountType }).ToList();

            return query.Select(x => x.account.AssignReferences(x.joinedCurrency, x.joinedAccountType));
        }
    }
}
