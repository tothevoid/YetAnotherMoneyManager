using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using MoneyManager.DAL.Database;
using MoneyManager.DAL.Entities;
using MoneyManager.DAL.Interfaces;
using MongoDB.Driver;

namespace MoneyManager.DAL.SpecificRepositories
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
