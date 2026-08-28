using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Audex.Infrastructure.Entities.Accounts;
using Audex.Infrastructure.Entities.Brokers;

namespace Audex.Infrastructure.Configurations.Brokers
{
    public class BrokerConfiguration: IEntityTypeConfiguration<Broker>
    {
        public void Configure(EntityTypeBuilder<Broker> accountConfiguration)
        {
        }
    }
}
