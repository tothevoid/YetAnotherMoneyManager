using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Audex.Infrastructure.Entities.Accounts;
using Audex.Infrastructure.Entities.Brokers;

namespace Audex.Infrastructure.Configurations.Brokers
{
    public class BrokerAccountTypeConfiguration : IEntityTypeConfiguration<BrokerAccountType>
    {
        public void Configure(EntityTypeBuilder<BrokerAccountType> accountConfiguration)
        {
        }
    }
}
