using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MoneyManager.Infrastructure.Configurations.Accounts;
using MoneyManager.Infrastructure.Configurations.Brokers;
using MoneyManager.Infrastructure.Configurations.Currencies;
using MoneyManager.Infrastructure.Configurations.Deposits;
using MoneyManager.Infrastructure.Configurations.Securities;
using MoneyManager.Infrastructure.Configurations.Transactions;
using MoneyManager.Infrastructure.Data;
using MoneyManager.Infrastructure.Entities.Accounts;
using MoneyManager.Infrastructure.Entities.Currencies;
using MoneyManager.Infrastructure.Entities.Securities;
using MoneyManager.Shared.Entities;
using AccountConfiguration = MoneyManager.Infrastructure.Configurations.Accounts.AccountConfiguration;

namespace MoneyManager.Infrastructure.Database
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            #if DEBUG
            optionsBuilder.EnableSensitiveDataLogging();
            #endif
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
            modelBuilder.ApplyConfiguration(new DividendConfiguration());
            modelBuilder.ApplyConfiguration(new TransactionConfiguration());
            modelBuilder.ApplyConfiguration(new TransactionTypeConfiguration());

            InitializeDefaultData(modelBuilder);

            base.OnModelCreating(modelBuilder);
        }

        private void InitializeDefaultData(ModelBuilder builder)
        {
            builder.Entity<Currency>().HasData(new CurrencyGenerator().Generate());
            builder.Entity<AccountType>().HasData(new AccountTypeGenerator().Generate());
            builder.Entity<SecurityType>().HasData(new SecurityTypeGenerator().Generate());
        }
    }
}
