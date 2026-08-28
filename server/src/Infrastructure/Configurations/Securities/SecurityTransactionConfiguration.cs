using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Audex.Infrastructure.Entities.Securities;

namespace Audex.Infrastructure.Configurations.Securities
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
