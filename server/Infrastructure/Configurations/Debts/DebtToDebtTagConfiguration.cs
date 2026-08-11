using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MoneyManager.Infrastructure.Entities.Debts;

namespace MoneyManager.Infrastructure.Configurations.Debts
{
    public class DebtToDebtTagConfiguration : IEntityTypeConfiguration<DebtToDebtTag>
    {
        public void Configure(EntityTypeBuilder<DebtToDebtTag> builder)
        {
            builder.HasOne(dt => dt.Debt)
                .WithMany(d => d.DebtTags)
                .HasForeignKey(dt => dt.DebtId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(dt => dt.DebtTag)
                .WithMany(t => t.DebtAssociations)
                .HasForeignKey(dt => dt.DebtTagId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
