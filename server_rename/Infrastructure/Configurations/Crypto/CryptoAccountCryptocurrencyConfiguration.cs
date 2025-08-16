using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using MoneyManager.Infrastructure.Entities.Securities;
using MoneyManager.Infrastructure.Entities.Crypto;

namespace MoneyManager.Infrastructure.Configurations.Securities
{
    public class CryptoAccountCryptocurrencyConfiguration : IEntityTypeConfiguration<CryptoAccountCryptocurrency>
    {
        public void Configure(EntityTypeBuilder<CryptoAccountCryptocurrency> cryptoAccountCryptocurrencyConfiguration)
        {
            cryptoAccountCryptocurrencyConfiguration
                .HasOne(cryptoAccountCryptocurrency => cryptoAccountCryptocurrency.Cryptocurrency)
                .WithMany(cryptocurrency => cryptocurrency.CryptoAccountCryptocurrencies)
                .HasForeignKey(cryptoAccountCryptocurrency => cryptoAccountCryptocurrency.CrpytocurrencyId)
                .OnDelete(DeleteBehavior.Restrict);

            cryptoAccountCryptocurrencyConfiguration
               .HasOne(cryptoAccountCryptocurrency => cryptoAccountCryptocurrency.CryptoAccount)
               .WithMany(cryptoAccount => cryptoAccount.CryptoAccountCryptocurrencies)
               .HasForeignKey(cryptoAccountCryptocurrency => cryptoAccountCryptocurrency.CryptoAccountId)
               .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
