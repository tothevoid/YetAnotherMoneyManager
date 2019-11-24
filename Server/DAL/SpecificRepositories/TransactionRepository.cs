using System.Threading.Tasks;
using MoneyManager.DAL.Interfaces;
using MongoDB.Driver;
using System.Collections.Generic;
using MoneyManager.Common;
using MoneyManager.DAL.Database;
using MoneyManager.DAL.Entities;
using MongoDB.Driver.Linq;
using System.Linq;
using System;

namespace MoneyManager.DAL.SpecificRepositories
{
    public class TransactionRepository: Repository<Transaction>, ITransactionRepository
    {
        public TransactionRepository(IMongoContext context): base(context){}

        public async Task<IEnumerable<Transaction>> GetAllFull(int month, int year)
        {
            var fundCollection = _context.GetCollection<Fund>(typeof(Fund).Name);
            var typeCollection = _context.GetCollection<TransactionType>(typeof(TransactionType).Name);
            var (stardDate, endDate) = GetDateRange(month, year);

            //TODO: fix AsEnumerable then mongo driver will support inner join
            var query = (from transaction in DbSet.AsQueryable()
                        .Where(x => x.Date >= stardDate && x.Date <= endDate).AsEnumerable()
                        join fund in fundCollection.AsQueryable() on transaction.FundSourceId equals fund.Id into fundJoin
                        from joinedFund in fundJoin.DefaultIfEmpty()
                        join type in typeCollection.AsQueryable() on transaction.TransactionTypeId equals type.Id into typeJoin
                        from joinedType in typeJoin.DefaultIfEmpty()

                        select new { transaction, joinedFund, joinedType }).ToList();

            return query.Select(x => x.transaction.AssignFund(x.joinedFund).AssignType(x.joinedType));
        }
    
        private (DateTime, DateTime) GetDateRange(int month, int year)
        {
            var startDate = new DateTime(year, month, 1);
            var endDate = new DateTime(year, month, 1).AddMonths(1).AddDays(-1);
            return (startDate, endDate);
        }
    }
}