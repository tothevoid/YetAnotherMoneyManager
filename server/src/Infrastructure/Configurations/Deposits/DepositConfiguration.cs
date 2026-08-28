using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Audex.Infrastructure.Entities.Deposits;

namespace Audex.Infrastructure.Configurations.Deposits
{
    public class DepositConfiguration : IEntityTypeConfiguration<Deposit>
    {
        public void Configure(EntityTypeBuilder<Deposit> depositConfiguration)
        {
            depositConfiguration
                .HasOne(deposit => deposit.Currency)
                .WithMany(currency => currency.Deposits)
                .HasForeignKey(deposit => deposit.CurrencyId)
                .OnDelete(DeleteBehavior.Restrict);

            depositConfiguration
                .HasOne(account => account.Bank)
                .WithMany(bank => bank.Deposits)
                .HasForeignKey(deposit => deposit.BankId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}