using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Audex.Infrastructure.Entities.Securities;
using Audex.Infrastructure.Entities.Crypto;

namespace Audex.Infrastructure.Configurations.Securities
{
    public class CryptoProviderConfiguration : IEntityTypeConfiguration<CryptoProvider>
    {
        public void Configure(EntityTypeBuilder<CryptoProvider> dividendConfiguration)
        {
        }
    }
}
