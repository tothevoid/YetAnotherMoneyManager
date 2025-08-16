using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MoneyManager.Infrastructure.Entities.Securities;

namespace MoneyManager.Infrastructure.Configurations.Securities
{
    public class SecurityTransactionConfiguration : IEntityTypeConfiguration<SecurityTransaction>
    {
        public void Configure(EntityTypeBuilder<SecurityTransaction> securityTransactionConfiguration)
        {
            securityTransactionConfiguration
                .HasOne(securityTransaction => securityTransaction.Security)
                .WithMany(security => security.SecurityTransactions)
                .HasForeignKey(securityTransaction => securityTransaction.SecurityId)
                .OnDelete(DeleteBehavior.Restrict);

            securityTransactionConfiguration
                .HasOne(securityTransaction => securityTransaction.BrokerAccount)
                .WithMany(brokerAccount => brokerAccount.SecurityTransactions)
                .HasForeignKey(securityTransaction => securityTransaction.BrokerAccountId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
