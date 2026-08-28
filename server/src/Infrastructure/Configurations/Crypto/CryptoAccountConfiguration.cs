using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Audex.Infrastructure.Entities.Securities;
using Audex.Infrastructure.Entities.Crypto;

namespace Audex.Infrastructure.Configurations.Securities
{
    public class CryptoAccountConfiguration : IEntityTypeConfiguration<CryptoAccount>
    {
        public void Configure(EntityTypeBuilder<CryptoAccount> cryptoAccountConfiguration)
        {
            cryptoAccountConfiguration
                .HasOne(cryptoAccount => cryptoAccount.CryptoProvider)
                .WithMany(provider => provider.CryptoAccounts)
                .HasForeignKey(cryptoAccount => cryptoAccount.CryptoProviderId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
