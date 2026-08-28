using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MoneyManager.Infrastructure.Configurations.Accounts;
using MoneyManager.Infrastructure.Entities.Debts;

namespace MoneyManager.Infrastructure.Configurations.Debts
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