using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Audex.Infrastructure.Entities.Debts;

namespace Audex.Infrastructure.Configurations.Debts
{
    public class DebtTagConfiguration : IEntityTypeConfiguration<DebtTag>
    {
        public void Configure(EntityTypeBuilder<DebtTag> builder)
        {
            builder.Property(tag => tag.Name).IsRequired().HasMaxLength(100);
            builder.Property(tag => tag.ColorHex).IsRequired().HasMaxLength(30);
        }
    }
}
