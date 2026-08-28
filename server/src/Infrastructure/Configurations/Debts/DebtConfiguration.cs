using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Audex.Infrastructure.Configurations.Accounts;
using Audex.Infrastructure.Entities.Debts;

namespace Audex.Infrastructure.Configurations.Debts
{
    public class DebtConfiguration : IEntityTypeConfiguration<Debt>
    {
        public void Configure(EntityTypeBuilder<Debt> debtConfiguration)
        {
            debtConfiguration
                .HasOne(debt => debt.Currency)
                .WithMany(currency => currency.Debts)
                .HasForeignKey(debt => debt.CurrencyId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}