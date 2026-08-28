using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Audex.Infrastructure.Entities.Securities;

namespace Audex.Infrastructure.Configurations.Securities
{
    public class SecurityConfiguration : IEntityTypeConfiguration<Security>
    {
        public void Configure(EntityTypeBuilder<Security> accountConfiguration)
        {
            accountConfiguration
                .HasOne(security => security.Type)
                .WithMany(type => type.Securities)
                .HasForeignKey(security => security.TypeId)
                .OnDelete(DeleteBehavior.Restrict);

            accountConfiguration
                .HasOne(security => security.Currency)
                .WithMany(currency => currency.Securities)
                .HasForeignKey(security => security.CurrencyId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
