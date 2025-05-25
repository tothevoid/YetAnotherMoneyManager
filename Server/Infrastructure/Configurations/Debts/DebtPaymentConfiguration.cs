using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MoneyManager.Infrastructure.Entities.Debts;

namespace MoneyManager.Infrastructure.Configurations.Debts
{
    public class DebtPaymentConfiguration : IEntityTypeConfiguration<DebtPayment>
    {
        public void Configure(EntityTypeBuilder<DebtPayment> debtPaymentConfiguration)
        {
            debtPaymentConfiguration.HasOne(x => x.Debt);
            debtPaymentConfiguration.HasOne(x => x.TargetAccount);
        }
    }
}