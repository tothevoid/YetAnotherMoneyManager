using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using MoneyManager.Infrastructure.Entities.Securities;

namespace MoneyManager.Infrastructure.Configurations.Securities
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
