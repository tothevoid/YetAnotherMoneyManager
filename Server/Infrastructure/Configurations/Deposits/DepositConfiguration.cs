using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MoneyManager.Infrastructure.Entities.Deposits;

namespace MoneyManager.Infrastructure.Configurations.Deposits
{
    public class DepositConfiguration : IEntityTypeConfiguration<Deposit>
    {
        public void Configure(EntityTypeBuilder<Deposit> accountConfiguration)
        {
            accountConfiguration
                .HasOne(deposit => deposit.Account)
                .WithMany(account => account.Deposits)
                .HasForeignKey(deposit => deposit.AccountId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}