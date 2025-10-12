using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MoneyManager.Infrastructure.Entities.Brokers;

namespace MoneyManager.Infrastructure.Configurations.Brokers
{
    public class BrokerAccountFundsTransferConfiguration : IEntityTypeConfiguration<BrokerAccountFundsTransfer>
    {
        public void Configure(EntityTypeBuilder<BrokerAccountFundsTransfer> configuration)
        {
            configuration
                .HasOne(brokerAccountFundsTransfer => brokerAccountFundsTransfer.BrokerAccount)
                .WithMany(brokerAccount => brokerAccount.BrokerAccountFundsTransfers)
                .HasForeignKey(brokerAccountFundsTransfer => brokerAccountFundsTransfer.BrokerAccountId)
                .OnDelete(DeleteBehavior.Restrict);

            configuration
                .HasOne(brokerAccountFundsTransfer => brokerAccountFundsTransfer.Account)
                .WithMany(account => account.BrokerAccountFundsTransfers)
                .HasForeignKey(brokerAccountFundsTransfer => brokerAccountFundsTransfer.AccountId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
