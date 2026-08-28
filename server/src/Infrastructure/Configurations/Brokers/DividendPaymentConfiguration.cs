using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MoneyManager.Infrastructure.Entities.Brokers;

namespace MoneyManager.Infrastructure.Configurations.Brokers
{
    public class DividendPaymentConfiguration : IEntityTypeConfiguration<DividendPayment>
    {
        public void Configure(EntityTypeBuilder<DividendPayment> dividendPaymentConfiguration)
        {
            dividendPaymentConfiguration
                .HasOne(dividendPayment => dividendPayment.BrokerAccount)
                .WithMany(brokerAccount => brokerAccount.DividendPayments)
                .HasForeignKey(dividendPayment => dividendPayment.BrokerAccountId)
                .OnDelete(DeleteBehavior.Restrict);
            
            dividendPaymentConfiguration
                .HasOne(dividendPayment => dividendPayment.Dividend)
                .WithMany(dividend => dividend.DividendPayments)
                .HasForeignKey(dividendPayment => dividendPayment.DividendId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}