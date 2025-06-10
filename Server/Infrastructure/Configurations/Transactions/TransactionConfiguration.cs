using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MoneyManager.Infrastructure.Entities.Transactions;

namespace MoneyManager.Infrastructure.Configurations.Transactions
{
    public class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
    {
        public void Configure(EntityTypeBuilder<Transaction> transactionConfiguration)
        {
            transactionConfiguration
                .HasOne(transaction => transaction.Account)
                .WithMany(account => account.Transactions)
                .HasForeignKey(transaction => transaction.AccountId)
                .OnDelete(DeleteBehavior.Restrict);

            transactionConfiguration
                .HasOne(transaction => transaction.TransactionType)
                .WithMany(transactionType => transactionType.Transactions)
                .HasForeignKey(transaction => transaction.TransactionTypeId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
