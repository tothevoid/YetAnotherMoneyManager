using System;
using System.Collections.Generic;
using System.Linq;
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

        public IEnumerable<Account> GetAllFull()
        {
            var currencyCollection = _context.GetCollection<Currency>(nameof(Currency));
            var accountTypeCollection = _context.GetCollection<AccountType>(nameof(AccountType));
            //TODO: fix AsEnumerable then mongo driver will support inner join
            var query = (from account in DbSet.AsQueryable()
                join currency in currencyCollection.AsQueryable() on account.CurrencyId equals currency.Id into currencyJoin
                from joinedCurrency in currencyJoin.DefaultIfEmpty()
                join accountType in accountTypeCollection.AsQueryable() on account.AccountType.Id equals accountType.Id into accountTypeJoin
                from joinedAccountType in accountTypeJoin.DefaultIfEmpty()
                select new { account, joinedCurrency, joinedAccountType }).ToList();

            return query.Select(x => x.account.AssignReferences(x.joinedCurrency, x.joinedAccountType));
        }
    }
}
