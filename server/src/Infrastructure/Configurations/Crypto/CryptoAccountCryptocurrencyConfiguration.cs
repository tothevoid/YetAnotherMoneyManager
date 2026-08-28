using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Audex.Infrastructure.Entities.Securities;
using Audex.Infrastructure.Entities.Crypto;

namespace Audex.Infrastructure.Configurations.Securities
{
    public class CryptoAccountCryptocurrencyConfiguration : IEntityTypeConfiguration<CryptoAccountCryptocurrency>
    {
        public void Configure(EntityTypeBuilder<CryptoAccountCryptocurrency> cryptoAccountCryptocurrencyConfiguration)
        {
            cryptoAccountCryptocurrencyConfiguration
                .HasOne(cryptoAccountCryptocurrency => cryptoAccountCryptocurrency.Cryptocurrency)
                .WithMany(cryptocurrency => cryptocurrency.CryptoAccountCryptocurrencies)
                .HasForeignKey(cryptoAccountCryptocurrency => cryptoAccountCryptocurrency.CryptocurrencyId)
                .OnDelete(DeleteBehavior.Restrict);

            cryptoAccountCryptocurrencyConfiguration
               .HasOne(cryptoAccountCryptocurrency => cryptoAccountCryptocurrency.CryptoAccount)
               .WithMany(cryptoAccount => cryptoAccount.CryptoAccountCryptocurrencies)
               .HasForeignKey(cryptoAccountCryptocurrency => cryptoAccountCryptocurrency.CryptoAccountId)
               .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
