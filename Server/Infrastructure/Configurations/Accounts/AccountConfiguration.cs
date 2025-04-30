using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MoneyManager.Infrastructure.Entities.Accounts;

namespace MoneyManager.Infrastructure.Configurations.Accounts
{
    public class AccountConfiguration: IEntityTypeConfiguration<Account>
    {
        public void Configure(EntityTypeBuilder<Account> accountConfiguration)
        {
            accountConfiguration.HasOne(x => x.Currency);
            accountConfiguration.HasOne(x => x.AccountType);
        }
    }
}
