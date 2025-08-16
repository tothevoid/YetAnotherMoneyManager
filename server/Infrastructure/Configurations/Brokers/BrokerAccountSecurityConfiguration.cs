using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MoneyManager.Infrastructure.Entities.Brokers;

namespace MoneyManager.Infrastructure.Configurations.Brokers
{
    public class BrokerAccountSecurityConfiguration : IEntityTypeConfiguration<BrokerAccountSecurity>
    {
        public void Configure(EntityTypeBuilder<BrokerAccountSecurity> accountConfiguration)
        {
            accountConfiguration
                .HasOne(brokerAccountSecurity => brokerAccountSecurity.BrokerAccount)
                .WithMany(brokerAccount => brokerAccount.BrokerAccountSecurities)
                .HasForeignKey(brokerAccountSecurity => brokerAccountSecurity.BrokerAccountId)
                .OnDelete(DeleteBehavior.Restrict);

            accountConfiguration
                .HasOne(brokerAccountSecurity => brokerAccountSecurity.Security)
                .WithMany(security => security.BrokerAccountSecurities)
                .HasForeignKey(brokerAccountSecurity => brokerAccountSecurity.SecurityId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
