using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MoneyManager.Infrastructure.Entities.Brokers;

namespace MoneyManager.Infrastructure.Configurations.Brokers
{
    public class BrokerAccountConfiguration: IEntityTypeConfiguration<BrokerAccount>
    {
        public void Configure(EntityTypeBuilder<BrokerAccount> accountConfiguration)
        {
            accountConfiguration
                .HasOne(brokerAccount => brokerAccount.Type)
                .WithMany(accountType => accountType.BrokerAccounts)
                .HasForeignKey(brokerAccount => brokerAccount.TypeId)
                .OnDelete(DeleteBehavior.Restrict);

            accountConfiguration
                .HasOne(brokerAccount => brokerAccount.Currency)
                .WithMany(currency => currency.BrokerAccounts)
                .HasForeignKey(brokerAccount => brokerAccount.CurrencyId)
                .OnDelete(DeleteBehavior.Restrict);

            accountConfiguration
                .HasOne(brokerAccount => brokerAccount.Broker)
                .WithMany(broker => broker.BrokerAccounts)
                .HasForeignKey(brokerAccount => brokerAccount.BrokerId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
