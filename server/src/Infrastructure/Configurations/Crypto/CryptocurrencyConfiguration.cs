using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Audex.Infrastructure.Entities.Securities;
using Audex.Infrastructure.Entities.Crypto;

namespace Audex.Infrastructure.Configurations.Securities
{
    public class CryptocurrencyConfiguration : IEntityTypeConfiguration<Cryptocurrency>
    {
        public void Configure(EntityTypeBuilder<Cryptocurrency> dividendConfiguration)
        {
        }
    }
}
