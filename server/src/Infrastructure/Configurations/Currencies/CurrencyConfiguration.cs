using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Audex.Infrastructure.Entities.Currencies;

namespace Audex.Infrastructure.Configurations.Currencies
{
    public class CurrencyConfiguration : IEntityTypeConfiguration<Currency>
    {
        public void Configure(EntityTypeBuilder<Currency> accountConfiguration)
        {
        }
    }
}