using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using MoneyManager.Infrastructure.Entities.Brokers;

namespace MoneyManager.Infrastructure.Configurations.Brokers
{
    public class BrokerAccountTaxDeductionConfiguration : IEntityTypeConfiguration<BrokerAccountTaxDeduction>
    {
        public void Configure(EntityTypeBuilder<BrokerAccountTaxDeduction> brokerAccountTaxDeduction)
        {
            brokerAccountTaxDeduction
                .HasOne(taxDeduction => taxDeduction.BrokerAccount)
                .WithMany(brokerAccount => brokerAccount.BrokerAccountTaxDeductions)
                .HasForeignKey(taxDeduction => taxDeduction.BrokerAccountId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
