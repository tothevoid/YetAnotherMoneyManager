using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Audex.Infrastructure.Entities.Debts;

namespace Audex.Infrastructure.Configurations.Debts
{
    public class DebtPaymentConfiguration : IEntityTypeConfiguration<DebtPayment>
    {
        public void Configure(EntityTypeBuilder<DebtPayment> debtPaymentConfiguration)
        {
            debtPaymentConfiguration
                .HasOne(debtPayment => debtPayment.Debt)
                .WithMany(debt => debt.DebtPayments)
                .HasForeignKey(debtPayment => debtPayment.DebtId)
                .OnDelete(DeleteBehavior.Restrict);

            debtPaymentConfiguration
                .HasOne(debtPayment => debtPayment.TargetAccount)
                .WithMany(account => account.DebtPayments)
                .HasForeignKey(debtPayment => debtPayment.TargetAccountId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}