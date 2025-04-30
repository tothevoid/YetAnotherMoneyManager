using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MoneyManager.Infrastructure.Configurations.Accounts;
using MoneyManager.Infrastructure.Configurations.Brokers;
using MoneyManager.Infrastructure.Configurations.Currencies;
using MoneyManager.Infrastructure.Configurations.Deposits;
using MoneyManager.Infrastructure.Configurations.Securities;
using MoneyManager.Infrastructure.Configurations.Transactions;
using AccountConfiguration = MoneyManager.Infrastructure.Configurations.Accounts.AccountConfiguration;

namespace MoneyManager.Infrastructure.Database
{
    public class ApplicationDbContext : DbContext
    {
        //private static bool _isDataInitialized;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            //var mongoClient = new MongoClient(configuration.GetSection("MongoDB").GetSection("ConnectionString").Value);
            //Database = mongoClient.GetDatabase(configuration.GetSection("MongoDB").GetSection("DatabaseName").Value);

            ////TODO: fix that workaround
            //if (!_isDataInitialized)
            //{
            //    InitializeDefaultData();
            //}
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new AccountConfiguration());
            modelBuilder.ApplyConfiguration(new AccountTypeConfiguration());
            modelBuilder.ApplyConfiguration(new BrokerConfiguration());
            modelBuilder.ApplyConfiguration(new BrokerAccountConfiguration());
            modelBuilder.ApplyConfiguration(new BrokerAccountSecurityConfiguration());
            modelBuilder.ApplyConfiguration(new BrokerAccountTypeConfiguration());
            modelBuilder.ApplyConfiguration(new CurrencyConfiguration());
            modelBuilder.ApplyConfiguration(new DepositConfiguration());
            modelBuilder.ApplyConfiguration(new SecurityConfiguration());
            modelBuilder.ApplyConfiguration(new SecurityTransactionConfiguration());
            modelBuilder.ApplyConfiguration(new SecurityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new TransactionConfiguration());
            modelBuilder.ApplyConfiguration(new TransactionTypeConfiguration());

            base.OnModelCreating(modelBuilder);
        }

        //private void InitializeDefaultData()
        //{
        //    var results = new bool[]
        //    {
        //        InitData(GetCollection<Currency>(nameof(Currency)), new CurrencyGenerator().Generate()),
        //        InitData(GetCollection<AccountType>(nameof(AccountType)), new AccountTypeGenerator().Generate()),
        //        InitData(GetCollection<SecurityType>(nameof(SecurityType)), new SecurityTypeGenerator().Generate())
        //    };

        //    if (results.Any(result => result))
        //    {
        //        SaveChanges();
        //    }

        //    _isDataInitialized = true;
        //}

        //private bool InitData<TEntity>(IMongoCollection<TEntity> collection, IEnumerable<TEntity> data)
        //    where TEntity : BaseEntity
        //{
        //    if (collection.Find(FilterDefinition<TEntity>.Empty).Any())
        //    {
        //        return false;
        //    }

        //    collection.InsertMany(data);

        //    return true;
        //}

        
    }
}
