using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MoneyManager.Infrastructure.Entities.Accounts;
using MoneyManager.Infrastructure.Entities.Brokers;

namespace MoneyManager.Infrastructure.Configurations.Brokers
{
    public class BrokerAccountConfiguration: IEntityTypeConfiguration<BrokerAccount>
    {
        public void Configure(EntityTypeBuilder<BrokerAccount> accountConfiguration)
        {
            accountConfiguration.HasOne(x => x.Type);
            accountConfiguration.HasOne(x => x.Currency);
            accountConfiguration.HasOne(x => x.Broker);
        }
    }
}
