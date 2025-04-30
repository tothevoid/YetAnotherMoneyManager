using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MoneyManager.Infrastructure.Entities.Accounts;
using MoneyManager.Infrastructure.Entities.Brokers;

namespace MoneyManager.Infrastructure.Configurations.Brokers
{
    public class BrokerConfiguration: IEntityTypeConfiguration<Broker>
    {
        public void Configure(EntityTypeBuilder<Broker> accountConfiguration)
        {
        }
    }
}
