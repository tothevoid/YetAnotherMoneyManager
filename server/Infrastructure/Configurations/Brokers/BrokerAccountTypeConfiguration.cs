using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MoneyManager.Infrastructure.Entities.Accounts;
using MoneyManager.Infrastructure.Entities.Brokers;

namespace MoneyManager.Infrastructure.Configurations.Brokers
{
    public class BrokerAccountTypeConfiguration : IEntityTypeConfiguration<BrokerAccountType>
    {
        public void Configure(EntityTypeBuilder<BrokerAccountType> accountConfiguration)
        {
        }
    }
}
