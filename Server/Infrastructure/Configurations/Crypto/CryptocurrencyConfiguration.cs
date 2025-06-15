using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using MoneyManager.Infrastructure.Entities.Securities;
using MoneyManager.Infrastructure.Entities.Crypto;

namespace MoneyManager.Infrastructure.Configurations.Securities
{
    public class CryptocurrencyConfiguration : IEntityTypeConfiguration<Cryptocurrency>
    {
        public void Configure(EntityTypeBuilder<Cryptocurrency> dividendConfiguration)
        {
        }
    }
}
