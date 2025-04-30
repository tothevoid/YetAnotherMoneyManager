using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MoneyManager.Infrastructure.Entities.Securities;

namespace MoneyManager.Infrastructure.Configurations.Securities
{
    public class SecurityTypeConfiguration : IEntityTypeConfiguration<SecurityType>
    {
        public void Configure(EntityTypeBuilder<SecurityType> accountConfiguration)
        {
        }
    }
}
