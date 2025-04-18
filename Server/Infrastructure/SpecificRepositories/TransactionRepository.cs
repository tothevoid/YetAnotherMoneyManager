﻿using MoneyManager.Infrastructure.Database;
using MoneyManager.Infrastructure.Entities.Accounts;
using MoneyManager.Infrastructure.Entities.Transactions;
using MoneyManager.Infrastructure.Interfaces.Database;
using MoneyManager.Infrastructure.Interfaces.Repositories;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoneyManager.Infrastructure.SpecificRepositories
{
    public class TransactionRepository: Repository<Transaction>, ITransactionRepository
    {
        public TransactionRepository(IMongoContext context): base(context){}

        public IEnumerable<Transaction> GetAllFull(int month, int year)
        {
            var accountCollection = _context.GetCollection<Account>(nameof(Account));
            var typeCollection = _context.GetCollection<TransactionType>(nameof(TransactionType));
            var (startDate, endDate) = GetDateRange(month, year);

            //TODO: fix AsEnumerable then mongo driver will support inner join
            var query = (from transaction in DbSet.AsQueryable()
                        .Where(x => x.Date >= startDate && x.Date <= endDate).AsEnumerable()
                        join account in accountCollection.AsQueryable() on transaction.AccountId equals account.Id into accountJoin
                        from joinedAccount in accountJoin.DefaultIfEmpty()
                        join type in typeCollection.AsQueryable() on transaction.TransactionTypeId equals type.Id into typeJoin
                        from joinedType in typeJoin.DefaultIfEmpty()

                        select new { transaction, joinedAccount, joinedType }).ToList();

            return query.Select(x => x.transaction.AssignAccount(x.joinedAccount));
            //.AssignType(x.joinedType));
        }

        public async Task<IEnumerable<string>> GetTypes()
        {
            return await DbSet
                .DistinctAsync<string>(nameof(Transaction.TransactionType), Builders<Transaction>.Filter.Empty)
                .Result.ToListAsync();
        }

        private (DateOnly, DateOnly) GetDateRange(int month, int year)
        {
            var startDate = new DateOnly(year, month, 1);
            var endDate = new DateOnly(year, month, 1).AddMonths(1).AddDays(-1);
            return (startDate, endDate);
        }
    }
}