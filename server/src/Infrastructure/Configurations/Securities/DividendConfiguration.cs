using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Audex.Infrastructure.Entities.Securities;

namespace Audex.Infrastructure.Configurations.Securities
{
    public class DividendConfiguration : IEntityTypeConfiguration<Dividend>
    {
        public void Configure(EntityTypeBuilder<Dividend> dividendConfiguration)
        {
            dividendConfiguration
                .HasOne(dividend => dividend.Security)
                .WithMany(security => security.Dividends)
                .HasForeignKey(dividend => dividend.SecurityId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
