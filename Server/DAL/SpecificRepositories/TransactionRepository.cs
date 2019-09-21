using System.Threading.Tasks;
using MoneyManager.DAL.Interfaces;
using MongoDB.Driver;
using System.Collections.Generic;
using MoneyManager.Common;
using MoneyManager.DAL.Database;
using MoneyManager.DAL.Entities;
using MongoDB.Driver.Linq;
using System.Linq;

namespace MoneyManager.DAL.SpecificRepositories
{
    public class TransactionRepository: Repository<Transaction>, ITransactionRepository
    {
        public TransactionRepository(IMongoContext context): base(context){}

        public async Task<IEnumerable<Transaction>> GetAllFull()
        {
            var fundCollection = _context.GetCollection<Fund>(typeof(Fund).Name);
            
            //TODO: fix AsEnumerable then mongo driver will support inner join
            var query = (from transaction in DbSet.AsQueryable().AsEnumerable()
                        join fund in fundCollection.AsQueryable() on transaction.FundSourceId equals fund.Id into joined
                        from joinedFund in joined.DefaultIfEmpty()
                        select new { transaction, joinedFund }).ToList();

            return query.Select(x => x.transaction.AssignFund(x.joinedFund));
        }
    }
}