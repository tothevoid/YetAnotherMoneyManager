using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MoneyManager.Infrastructure.Entities.Currencies;
using MoneyManager.Infrastructure.Entities.Deposits;

namespace MoneyManager.Infrastructure.Configurations.Deposits
{
    public class DepositConfiguration : IEntityTypeConfiguration<Deposit>
    {
        public void Configure(EntityTypeBuilder<Deposit> accountConfiguration)
        {
            accountConfiguration.HasOne(x => x.Account);
        }
    }
}