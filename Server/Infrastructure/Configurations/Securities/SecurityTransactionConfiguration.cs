using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MoneyManager.Infrastructure.Entities.Securities;

namespace MoneyManager.Infrastructure.Configurations.Securities
{
    public class SecurityTransactionConfiguration : IEntityTypeConfiguration<SecurityTransaction>
    {
        public void Configure(EntityTypeBuilder<SecurityTransaction> accountConfiguration)
        {
            accountConfiguration.HasOne(x => x.Security);
            accountConfiguration.HasOne(x => x.BrokerAccount);
        }
    }
}
