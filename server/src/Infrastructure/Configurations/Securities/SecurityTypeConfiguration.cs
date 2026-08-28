using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Audex.Infrastructure.Entities.Securities;

namespace Audex.Infrastructure.Configurations.Securities
{
    public class SecurityTypeConfiguration : IEntityTypeConfiguration<SecurityType>
    {
        public void Configure(EntityTypeBuilder<SecurityType> accountConfiguration)
        {
        }
    }
}
