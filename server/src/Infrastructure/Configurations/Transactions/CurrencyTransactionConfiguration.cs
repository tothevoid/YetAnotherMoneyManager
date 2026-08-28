using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Audex.Infrastructure.Entities.Transactions;

namespace Audex.Infrastructure.Configurations.Transactions
{
    public class CurrencyTransactionConfiguration : IEntityTypeConfiguration<CurrencyTransaction>
    {
        public void Configure(EntityTypeBuilder<CurrencyTransaction> currencyTransactionConfiguration)
        {
            currencyTransactionConfiguration
                .HasOne(currencyTransaction => currencyTransaction.SourceAccount)
                .WithMany(account => account.SourceCurrencyTransactions)
                .HasForeignKey(currencyTransaction => currencyTransaction.SourceAccountId)
                .OnDelete(DeleteBehavior.Restrict);

            currencyTransactionConfiguration
                .HasOne(currencyTransaction => currencyTransaction.DestinationAccount)
                .WithMany(account => account.DestinationCurrencyTransactions)
                .HasForeignKey(currencyTransaction => currencyTransaction.DestinationAccountId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
