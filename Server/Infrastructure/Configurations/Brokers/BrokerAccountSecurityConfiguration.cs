using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MoneyManager.Infrastructure.Entities.Accounts;
using MoneyManager.Infrastructure.Entities.Brokers;

namespace MoneyManager.Infrastructure.Configurations.Brokers
{
    public class BrokerAccountSecurityConfiguration : IEntityTypeConfiguration<BrokerAccountSecurity>
    {
        public void Configure(EntityTypeBuilder<BrokerAccountSecurity> accountConfiguration)
        {
            accountConfiguration.HasOne(x => x.BrokerAccount);
            accountConfiguration.HasOne(x => x.Security);
        }
    }
}
